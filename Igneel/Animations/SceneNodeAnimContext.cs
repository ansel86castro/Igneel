using Igneel.Assets;
using Igneel.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{
    [Serializable]
    public class SceneNodeTransforms
    {         
        internal Matrix outPose = Matrix.Identity;        
        internal Vector3 outTraslation;
        internal Vector3 outScale = new Vector3(1, 1, 1);
        internal Quaternion outRotation;        
        internal bool usePose;
        internal bool useRotation;                      

        #region Tranformation Data

        public void ApplyRotation(Matrix rot, bool rightSide = true)
        {
            var rotQuad = Quaternion.RotationMatrix(rot);
            if (rightSide)
                outRotation *= rotQuad;
            else
                outRotation = rotQuad * outRotation;
            useRotation = true;          
        }

        public void ApplyRotation(Quaternion rot, bool rightSide = true)
        {
            if (rightSide)
                outRotation *= rot;
            else
                outRotation = rot * outRotation;
            useRotation = true;          
        }

        public Vector3 Traslation
        {
            get { return outTraslation; }
            set { outTraslation = value; }
        }

        public float TraslationX
        {
            get { return outTraslation.X; }
            set { outTraslation.X = value; }
        }

        public float TraslationY
        {
            get { return outTraslation.Y; }
            set { outTraslation.Y = value; }
        }

        public float TraslationZ
        {
            get { return outTraslation.Z; }
            set { outTraslation.Z = value; }
        }

        public Vector3 Scale
        {
            get { return outScale; }
            set
            {
                outScale = value;              
            }
        }

        public float ScalingX
        {
            get { return outScale.X; }
            set
            {
                outScale.X = value;
            }
        }

        public float ScalingY
        {
            get { return outScale.Y; }
            set
            {
                outScale.Y = value;
               
            }
        }

        public float ScalingZ
        {
            get { return outScale.Z; }
            set
            {
                outScale.Z = value;              
            }
        }

        public Matrix Pose
        {
            get { return outPose; }
            set
            {
                outPose = value;
                usePose = true;
            }
        }

        public Quaternion Rotation 
        {
            get { return outRotation; }
            set
            {
                outRotation = value;
                useRotation = true;
            }
        }
        #endregion     

        public SceneNodeTransforms Clone()
        {
            return (SceneNodeTransforms)MemberwiseClone();
        }
    }       

    
    [ProviderActivator(typeof(SceneNodeAnimContext.Activator))]
    public class SceneNodeAnimContext : IAnimContext<SceneNodeTransforms>, ITargetNamedContext
    {
        Matrix iniLocalPose;
        Vector3 iniTraslation;
        Vector3 iniScale;
        Quaternion iniRotation;

        SceneNode target;
        string targetName;             
        SceneNodeAnimContext next;      
        SceneNodeTransforms transforms = new SceneNodeTransforms();       

        public SceneNodeAnimContext() 
        {            
          
        }

        public SceneNodeAnimContext(SceneNode node):this()
        {
            target = node;           
            if (target != null)
                CaptureTargetState();
        }       

        [AssetMember]
        public string TargetName
        {
            get { return target != null ? targetName = target.Name : targetName; }
            set
            {
                targetName = value;
            }
        }       

        [AssetMember(storeAs: StoreType.Reference)]
        public SceneNodeAnimContext Next
        {
            get
            {
                return next;
            }
            set
            {
                next = value;               
            }
        }

        IAnimContext<SceneNodeTransforms> IAnimContext<SceneNodeTransforms>.Next
        {
            get
            {
                return next;
            }
            set
            {
                Next = (SceneNodeAnimContext)value;
            }
        }

        IAnimContext IAnimContext.Next
        {
            get
            {
                return next;
            }
            set
            {
                Next = (SceneNodeAnimContext)value;
            }
        }
      
        object IAnimContext.Target
        {
            get { return target; }
            set
            {
                Target = (SceneNode)value;               
            }
        }

        [AssetMember(storeAs: StoreType.Reference)]
        public SceneNode Target
        {
            get { return target; }
            set
            {
                target = value;
                if (target != null)
                {
                    targetName = target.Name;                   
                    CaptureTargetState();
                }
            }
        }

        public SceneNodeTransforms Data
        {
            get { return transforms; }
        }

        public void CaptureTargetState()
        {
            if (target == null)
            {
                if (targetName == null)
                    throw new InvalidOperationException("The target node can not be located");
                target = Engine.Scene.GetNode(targetName);
            }

            iniTraslation =  target.LocalPosition;
            iniScale = target.LocalScale;
            iniLocalPose = target.LocalPose;
            iniRotation = target.LocalRotationQuat;

            transforms.outRotation = Quaternion.Identity;
            transforms.outScale = iniScale;
            transforms.outTraslation = iniTraslation;

            if (next != null)
                next.CaptureTargetState();
        }

        public void RestoreTargetState()
        {           
            target.LocalPose = iniLocalPose;
            if (next != null)
                next.RestoreTargetState();
        }

        public void OnSample(bool isBlended, float blendWeight = 1.0f)
        {
            var rotationQuad = transforms.outRotation;
            var translation = transforms.outTraslation;
            var scale = transforms.outScale;

            if (!transforms.useRotation)
                rotationQuad = iniRotation;

            if (!isBlended)
            {
                if (transforms.usePose)
                   target.LocalPose = transforms.outPose;                
                else
                    target.ComputeLocalPose(scale, rotationQuad, translation);
            }
            else
            {
                if (transforms.usePose)
                {
                    Matrix diff = transforms.outPose;
                    diff -= iniLocalPose;
                    diff *= blendWeight;
                    target.LocalPose += diff;
                }
                else
                {
                    target.LocalScale += (scale - iniScale) * blendWeight;
                    target.LocalRotationQuat += (rotationQuad - iniRotation) * blendWeight;
                    target.LocalPosition += (translation - iniTraslation) * blendWeight;
                    target.UpdateLocalPose();
                }
            }

            //Reset State
            transforms.outPose = Matrix.Identity;
            transforms.outTraslation = iniTraslation;
            transforms.outScale = iniScale;
            transforms.outRotation = Quaternion.Identity;
            transforms.usePose = false;
            transforms.useRotation = false;
            
            if (next != null)
            {
                next.OnSample(isBlended, blendWeight);
            }
        }
    
        public override string ToString()
        {
            if (target != null)
                return target.ToString();
            if (targetName != null)
                return targetName;

            return base.ToString();
        }       

        public Asset CreateAsset()
        {
            return Asset.Create(this);
        }          

        public IAnimContext Clone()
        {
            SceneNodeAnimContext clone = (SceneNodeAnimContext)MemberwiseClone();
            clone.transforms = transforms.Clone();
            if (next != null)
                clone.Next = (SceneNodeAnimContext)next.Clone();
            return clone;
        }      

        [Serializable]
        class Activator : IProviderActivator
        {
            SceneNodeTransforms transforms;
            AssetReference nodeRef;

            public void Initialize(IAssetProvider provider)
            {
                var context = (SceneNodeAnimContext)provider;
                transforms = context.transforms;
                nodeRef = AssetManager.Instance.GetAssetReference(context.target);
            }

            public IAssetProvider CreateInstance()
            {
                var node = (SceneNode)AssetManager.Instance.GetAssetProvider(nodeRef);
                var context = (SceneNodeAnimContext)AnimationManager.GetContext<SceneNodeTransforms>(node);                
                context.targetName = node.Name;
                context.transforms = transforms;

                return context;
            }
        }       
    }
}
