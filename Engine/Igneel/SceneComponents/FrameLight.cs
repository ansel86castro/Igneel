using System;
using System.Collections.Generic;
using Igneel.Assets;
using Igneel.Components;
using Igneel.Graphics;
using Igneel.SceneManagement;
using Igneel.States;
using Igneel.Techniques;

namespace Igneel.SceneComponents
{
    [Asset("FRAME_OBJECT")]
    [ResourceActivator(typeof(FrameLight.Activator))]
    public class FrameLight : ComponentInstance, IRenderInput, IBoundable
    {
        bool _isActive;
        Light _light;
        Vector3 _localDirection = new Vector3(0, 0, 1);
        Vector3 _localPosition = new Vector3(0, 0, 0);
        Vector3 _globalDirection;
        Vector3 _globalPosition;
        private bool _isSync;     
     
        public FrameLight(Light light = null)
        {
            if (light != null)
            {
                OnLightChanging(light);
                this._light = light;
            }
            var srv = Service.Get<INotificationService>();
            if (srv != null)
                srv.OnObjectCreated(this);
        }

        public bool IsActive { get { return _isActive; } internal set { _isActive = value; } }
      
        [AssetMember]                
        public Vector3 LocalDirection { get { return _localDirection; } set { _localDirection = value; _isSync = false; } }

        [AssetMember]        
        public Vector3 LocalPosition { get { return _localPosition; } set { _localPosition = value; _isSync = false; } }

        public Vector3 GlobalDirection { get { return _globalDirection; } }
                
        public Vector3 GlobalPosition { get { return _globalPosition; } }

        public Sphere BoundingSphere { get; set; }

        public OrientedBox BoundingBox { get; set; }

        public Scene Scene { get; private set; }

        [AssetMember(storeAs:StoreType.Reference)]
        public Light Light
        {
            get { return _light; }
            set
            {
                if (_light != value)
                    OnLightChanging(value);
                _light = value;
            }
        }

        public bool IsGpuSync
        {
            get
            {
                return _isSync && _light.IsGpuSync;
            }
            set
            {
                _isSync = value;
                _light.IsGpuSync = value;
            }
        }

        void light_EffectiveRangeChanged(Light obj)
        {
            this.BoundingSphere = new Sphere(Vector3.Zero, obj.EffectiveRange);

            if (Node == null)
                return;

            Node.LocalScale = new Vector3(obj.EffectiveRange);            
            Node.CommitChanges();
        }

        private void OnLightChanging(Light value)
        {
            if (value != null)
            {              
                value.EffectiveRangeChanged += light_EffectiveRangeChanged;
                BoundingSphere = new Sphere(_localPosition, value.EffectiveRange);
            }
            else if (_light != null)
            {              
                _light.EffectiveRangeChanged -= light_EffectiveRangeChanged;
                BoundingSphere = new Sphere(_localPosition, 0);
            }
        }

        public ShaderLight GetShaderLight()
        {
            ShaderLight sl = _light;
            sl.Pos = _globalPosition;
            sl.Dir = _globalDirection;
            return sl;
        }

        public override void OnNodeAttach(Frame node)
        {                                
            OnPoseUpdated(node);            
            base.OnNodeAttach(node);
        }        

        public override void OnPoseUpdated(Frame node)
        {            
            _globalDirection = Vector3.TransformNormal(_localDirection, node.GlobalPose);
            _globalPosition = Vector3.Transform(_localPosition, node.GlobalPose);

            _isSync = false;
        }

        public override void OnSceneAttach(Scene scene)
        {
            Scene = scene;
            if (scene != null)
            {              
                scene.FrameLights.Add(this);             
            }
            base.OnSceneAttach(scene);
        }

        public override void OnSceneDetach(Scene scene)
        {
            Scene = null;
            if (scene != null)
            {
                scene.FrameLights.Remove(this);
            }
            base.OnSceneDetach(scene);
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

        public override int SubmitGraphics(Scene scene, Frame node, ICollection<GraphicSubmit> collection)
        {
            _isActive = false;         
            var camera = scene.ActiveCamera;

            if (_light.Enable && camera != null && camera.Contains(_globalPosition, _light.EffectiveRange))
            {
                scene.ActiveLights.Add(this);
                _isActive = true;
            }
            return 0;
        }

        public ShadowMapTechnique CreateShadowMap(Scene scene, int size, bool isDynamic, Vector3 maxWorld, Vector3 minWorld)
        {
            if (scene == null)
                throw new NullReferenceException("Scene");

            var sm = new ShadowMapTechnique(size ) { IsDynamic = isDynamic };
            var viewMat = Matrix.LookAt(_globalPosition, _globalPosition + _globalDirection, Vector3.UnitY);            
            var camera = new Camera();            
            camera.View = viewMat;

            if (_light.Type == LightType.Directional)
            {
                var centerWorld = 0.5f * (maxWorld + minWorld);
                float distance = Vector3.Distance(minWorld, maxWorld) * 0.5f;
                var newPosition = centerWorld - _globalDirection * distance;
                camera.Position = newPosition;
                viewMat = camera.View;
            }
       
            Vector3 maxView = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 minView = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            foreach (var n in scene.Geometries)
            {
                var meshContainer = n.Component as FrameMesh;
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

                FrameSkin skin = n.Component as FrameSkin;
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

            sm.OnSceneAttach(scene);

            return sm;
        }

        public ShadowMapTechnique CreateShadowMap(int size = 1024, bool dynamic =true)
        {
            if (Node.Technique != null && !(Node.Technique is ShadowMapTechnique))
            {
                return null;
            }

            var scene = Scene;
            var camera = new Camera(this.Name+"_shadowMap", 1, 1000);
            var initialPos = Vector3.Zero;
            var view = Matrix.LookAt(initialPos, initialPos + _globalDirection, Vector3.UnitY);
            camera.View = view;

            
            var sm =Node.Technique as ShadowMapTechnique ?? new ShadowMapTechnique(size) { IsDynamic = dynamic };

            if (Light.Type == LightType.Directional)
            {
                //Get bounding box in camera space
                scene.GetBoundingBox(out Vector3 viewMin, out Vector3 viewMax, camera.View);

                //1 compute initial shadow map camera position
                initialPos = new Vector3((viewMin.X + viewMax.X) * 0.5f, (viewMin.Y + viewMax.Y) * 0.5f, viewMin.Z - 0.1f);
                var invView = Matrix.Invert(view);
                initialPos = Vector3.TransformCoordinates(initialPos, invView);

                view = Matrix.LookAt(initialPos, initialPos + _globalDirection, Vector3.UnitY);
                camera.View = view;

                camera.ZNear = 0.1f;
                camera.ZFar = Math.Abs(viewMax.Z - viewMin.Z);
                camera.OrthoWidth = Math.Abs(viewMax.X - viewMin.X);
                camera.OrthoHeight = Math.Abs(viewMax.Y - viewMin.Y);
                camera.FieldOfView = Numerics.PIover2;

                camera.Type = ProjectionType.Orthographic;
                camera.CommitChanges();
            }           

            sm.Camera = camera;    
            
            if (Node.Technique == null)
            {
                sm.Affector = Node;
                Node.Technique = sm;
                sm.OnSceneAttach(scene);
            }
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
                FrameSkin skin = n.Component as FrameSkin;
                if (skin != null)
                {
                    TransformSkin(skin.Skin, ref minWorld, ref maxWorld , Matrix.Identity);
                }

                FrameMesh meshContainer = n.Component as FrameMesh;
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

            foreach (var lightInstance in scene.FrameLights)
            {
                lightInstance.CreateShadowMap(scene, EngineState.Shadow.ShadowMapping.Size, true, maxWorld, minWorld);
            }


            if (!EngineState.Shadow.ShadowMapping.Enable)
            {
                EngineState.Shadow.ShadowMapping.Enable = true;
                EngineState.Shadow.Enable = true;
            }
        }

        public static void TransformSkin(MeshSkin skin, ref Vector3 min, ref Vector3 max, Matrix view)
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
                    var partBones = skin.GetBones(part);
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
        class Activator : IResourceActivator
        {

            public void OnAssetCreated(object provider, ResourceOperationContext context)
            {
               
            }

            public object OnCreateResource(ResourceOperationContext context)
            {
                return null;
            }
        }

       
    }

}
