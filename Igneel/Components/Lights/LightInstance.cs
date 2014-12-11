using Igneel.Animations;
using Igneel.Assets;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{   
    [ProviderActivator(typeof(LightInstance.Activator))]
    public class LightInstance : ExclusiveNodeObject, IInstanceContainer<Light>,IShadingInput
    {
        bool isActive;
        Light light;
        Vector3 localDirection = new Vector3(0, 0, 1);
        Vector3 localPosition = new Vector3(0, 0, 0);
        Vector3 globalDirection;
        Vector3 globalPosition;
        private bool isSync;     
     
        public LightInstance(Light light = null)
        {
            if (light != null)
            {
                OnLightChanging(light);
                this.light = light;
            }
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }

        public bool IsActive { get { return isActive; } internal set { isActive = value; } }
      
        [AssetMember]
        [TypeConverter(typeof(Igneel.Design.DesignTypeConverter))]
        [Editor(typeof(Igneel.Design.UITypeEditors.UIDirectionEditor),typeof(UITypeEditor))]
        public Vector3 LocalDirection { get { return localDirection; } set { localDirection = value; isSync = false; } }

        [AssetMember]
        [Editor(typeof(Igneel.Design.UITypeEditors.UIVector3TypeEditor), typeof(UITypeEditor))]
        public Vector3 LocalPosition { get { return localPosition; } set { localPosition = value; isSync = false; } }

        public Vector3 GlobalDirection { get { return globalDirection; } }
                
        public Vector3 GlobalPosition { get { return globalPosition; } }

        [AssetMember(storeAs:StoreType.Reference)]
        public Light Instance
        {
            get { return light; }
            set
            {
                if (light != value)
                    OnLightChanging(value);
                light = value;
            }
        }

        public bool IsGPUSync
        {
            get
            {
                return isSync && light.IsGPUSync;
            }
            set
            {
                isSync = value;
                light.IsGPUSync = value;
            }
        }

        void light_EffectiveRangeChanged(Light obj)
        {
            sphere = new Sphere(Vector3.Zero, obj.EffectiveRange);

            if (Node == null)
                return;

            Node.LocalRadius = light.EffectiveRange;
            Node.CommitChanges();
        }        

        private void OnLightChanging(Light value)
        {
            if (value != null)
            {              
                value.EffectiveRangeChanged += light_EffectiveRangeChanged;
                sphere = new Sphere(localPosition, value.EffectiveRange);
            }
            else if (light != null)
            {              
                light.EffectiveRangeChanged -= light_EffectiveRangeChanged;
                sphere = new Sphere(localPosition, 0);
            }
        }

        public ShaderLight GetShaderLight()
        {
            ShaderLight sl = light;
            sl.Pos = globalPosition;
            sl.Dir = globalDirection;
            return sl;
        }

        public override void OnNodeAttach(SceneNode node)
        {                      
            node.LocalSphere = sphere;
            OnPoseUpdated(node);            
            base.OnNodeAttach(node);
        }        

        public override void OnPoseUpdated(SceneNode node)
        {            
            globalDirection = Vector3.TransformNormal(localDirection, node.GlobalPose);
            globalPosition = Vector3.Transform(localPosition, node.GlobalPose);

            isSync = false;
        }

        public override void OnAddToScene(Scene scene)
        {
            if (scene != null)
            {
                Engine.Lock();

                scene.Lights.Add(this);

                Engine.Unlock();
            }
            base.OnAddToScene(scene);
        }

        public override void OnRemoveFromScene(Scene scene)
        {
             if (scene != null)
            {
                Engine.Lock();

                scene.Lights.Remove(this);

                Engine.Unlock();
            }
             base.OnRemoveFromScene(scene);
        }

        protected override void OnDispose(bool d)
        {
            if (d)
            {              
                var srv = Service.Get<INotificationService>();
                if (srv != null)
                    srv.OnObjectDestroyed(this);
            }
            base.OnDispose(d);
        }

        public override int GetGraphicObjects(SceneNode node, ICollection<DrawingEntry> collection)
        {
            isActive = false;
            var scene = Engine.Scene;
            Sphere sphere = new Sphere(globalPosition, light.EffectiveRange);
            if (light.Enable && scene.ActiveCamera!=null && scene.ActiveCamera.ViewFrustum.Contains(sphere))
            {
                scene.ActiveLights.Add(this);
                isActive = true;
            }
            return 0;
        }

        public ShadowMapTechnique CreateShadowMap(Scene scene, int size, bool isDynamic, Vector3 maxWorld, Vector3 minWorld)
        {
            if (scene == null)
                throw new NullReferenceException("Scene");

            var sm = new ShadowMapTechnique(size ) { IsDynamic = isDynamic };
            var viewMat = Matrix.LookAt(globalPosition, globalPosition + globalDirection, Vector3.UnitY);            
            var camera = new Camera();            
            camera.View = viewMat;
            if (light.Type == LightType.Directional)
            {
                var centerWorld = 0.5f * (maxWorld + minWorld);
                float distance = Vector3.Distance(minWorld, maxWorld) * 0.5f;
                var newPosition = centerWorld - globalDirection * distance;
                camera.Position = newPosition;
                viewMat = camera.View;
            }
       
            Vector3 maxView = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 minView = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            foreach (var n in scene.Geometries)
            {
                MeshInstance meshContainer = n.NodeObject as MeshInstance;
                if (meshContainer != null)
                {
                    Mesh mesh = meshContainer.Mesh;
                    var globalPose = n.GlobalPose;
                    var pose =  globalPose * viewMat;
                    var positionView = mesh.GetVertexBufferView<Vector3>(IASemantic.Position);

                    for (int i = 0; i < positionView.Count; i++)
                    {
                        var pos = positionView[i];                        
                        var posView = Vector3.TransformCoordinates(pos, pose);

                        maxView = Vector3.Max(posView, maxView);
                        minView = Vector3.Min(posView, minView);                                      
                    }                    

                    mesh.ReleaseViews();
                }

                SkinInstance skin = n.NodeObject as SkinInstance;
                if (skin != null)
                {
                    TransformSkin(skin.Skin, ref minView, ref maxView, viewMat);
                }
            }            

            camera.ZNear = Math.Max(1, minView.Z);
            camera.ZFar = Math.Max(2, maxView.Z);
            camera.OrthoWidth = maxView.X - minView.X;
            camera.OrthoHeight = maxView.Y - minView.Y;
            camera.FieldOfView = Numerics.PIover2;           

            camera.Type = ProjectionType.Orthographic;
            sm.Camera = camera;
            sm.Affector = Node;
            Node.Technique = sm;

            sm.OnAddToScene(scene);

            return sm;
        }

        public static void CreateShadowMapForAllLights(Scene scene)
        {
            if (scene == null)
                throw new ArgumentNullException("scene");

            Vector3 maxWorld = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 minWorld = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            foreach (var n in scene.Geometries)
            {
                SkinInstance skin = n.NodeObject as SkinInstance;
                if (skin != null)
                {
                    TransformSkin(skin.Skin, ref minWorld, ref maxWorld , Matrix.Identity);
                }

                MeshInstance meshContainer = n.NodeObject as MeshInstance;
                if (meshContainer != null)
                {
                    Mesh mesh = meshContainer.Mesh;
                    var globalPose = n.GlobalPose;                 
                    var positionView = mesh.GetVertexBufferView<Vector3>(IASemantic.Position);
                    for (int i = 0; i < positionView.Count; i++)
                    {
                        var posWorld = Vector3.TransformCoordinates(positionView[i], globalPose);                                                
                        maxWorld = Vector3.Max(posWorld, maxWorld);
                        minWorld = Vector3.Min(posWorld, minWorld);
                    }

                    mesh.ReleaseViews();
                }
            }

            foreach (var lightInstance in scene.Lights)
            {
                lightInstance.CreateShadowMap(scene, Engine.Shadow.ShadowMapping.Size, true, maxWorld, minWorld);
            }


            if (!Engine.Shadow.ShadowMapping.Enable)
            {
                Engine.Shadow.ShadowMapping.Enable = true;
                Engine.Shadow.Enable = true;
            }
        }

        public static void TransformSkin(SkinDeformer skin, ref Vector3 min, ref Vector3 max, Matrix view)
        {
            var mesh = skin.Mesh;
            var positions = mesh.GetVertexBufferView<Vector3>(IASemantic.Position);
            var boneIndices = mesh.GetVertexBufferView<Vector4>(IASemantic.BlendIndices);
            var boneWeights = mesh.GetVertexBufferView<Vector4>(IASemantic.BlendWeight);           
            var bones = skin.Bones;
            var boneOffets = skin.BoneBindingMatrices;

            if (skin.HasBonesPerLayer)
            {
                foreach (var part in mesh.Layers)
                {
                    var partBones = skin.GetLayerBones(part);
                    for (int i = 0; i < part.VertexCount; i++)
                    {
                        var pos = Vector3.TransformCoordinates(positions[part.StartVertex + i], skin.BindShapePose);
                        var blendIndices = boneIndices[part.StartVertex + i];
                        var blendWeights = boneWeights[part.StartVertex + i];
                        Vector3 posWorld = new Vector3();
                        float lastWeight = 0;
                        unsafe
                        {
                            float* pIndices = (float*)&blendIndices;
                            float* pWeights = (float*)&blendWeights;
                            int ibone = 0;

                            for (int k = 0; k < 3; k++)
                            {
                                lastWeight += pWeights[k];
                                ibone = partBones[(int)pIndices[k]];

                                posWorld += Vector3.TransformCoordinates(pos,
                                    boneOffets[ibone] * bones[ibone].GlobalPose) * pWeights[k];
                            }

                            lastWeight = 1.0f - lastWeight;
                            ibone = partBones[(int)pIndices[3]];
                            posWorld += Vector3.TransformCoordinates(pos,
                                    boneOffets[ibone] * bones[ibone].GlobalPose) * pWeights[3];
                        }

                        posWorld = Vector3.TransformCoordinates(posWorld, view);
                        min = Vector3.Min(min, posWorld);
                        max = Vector3.Max(max, posWorld);
                    }
                }
            }
            else
            {
                for (int i = 0; i < positions.Count; i++)
                {
                    var pos = Vector3.TransformCoordinates(positions[i], skin.BindShapePose);
                    var blendIndices = boneIndices[i];
                    var blendWeights = boneWeights[i];
                    Vector3 posWorld = new Vector3();
                    float lastWeight = 0;
                    unsafe
                    {
                        float* pIndices = (float*)&blendIndices;
                        float* pWeights = (float*)&blendWeights;
                        int ibone = 0;

                        for (int k = 0; k < 3; k++)
                        {
                            lastWeight += pWeights[k];
                            ibone = (int)pIndices[k];

                            posWorld += Vector3.TransformCoordinates(pos,
                                boneOffets[ibone] * bones[ibone].GlobalPose) * pWeights[k];
                        }

                        lastWeight = 1.0f - lastWeight;
                        ibone = (int)pIndices[3];
                        posWorld += Vector3.TransformCoordinates(pos,
                                boneOffets[ibone] * bones[ibone].GlobalPose) * pWeights[3];
                    }

                    posWorld = Vector3.TransformCoordinates(posWorld, view);

                    min = Vector3.Min(min, posWorld);
                    max = Vector3.Max(max, posWorld);
                }
            }

            mesh.ReleaseViews();
        }

        [Serializable]
        class Activator : IProviderActivator
        {

            public void Initialize(IAssetProvider provider)
            {

            }

            public IAssetProvider CreateInstance()
            {
                return new LightInstance();
            }
        }        
    }

}
