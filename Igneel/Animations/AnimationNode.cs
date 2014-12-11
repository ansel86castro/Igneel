using Igneel.Assets;
using Igneel.Collections;
using Igneel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{   
    /// <summary>
    /// Contains Several AnimationCurves for animating an set of objects properties. All the objects must
    /// share the same set of animated properties.
    /// Some propertis may be for example  translation, rotation and scale
    /// The setting of the object properties is handled through an AnimationContext
    /// </summary>
    [OnComplete("OnComplete")]
    [ProviderActivator(typeof(AnimationNode.Activator))]
    public class AnimationNode : INameable, IAssetProvider
    {
        string name;
        float[][] curveKeys;
        AnimationCurve[] curves;
        int[] keysIndices;
        SampleInfo[] keySamples;
        IAnimContext context;       

        Animation  animation;
        private float lastKey = -1;
    
        public AnimationNode(string name)
        {
            this.name = name;
            var noti = Service.Get<INotificationService>();
            if (noti != null)
                noti.OnObjectCreated(this);
        }

        public AnimationNode() : this(null) { }     

        [AssetMember]
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value && animation != null)
                {
                    animation.Nodes.ChangeKey(name, value);                                    
                }

                name = value;
            }
        }

        public Animation Animation
        {
            get { return animation; }
            set { animation = value; }
        }

        [AssetMember(typeof(ArrayStoreConverter<AnimationCurve>))]
        public AnimationCurve[] Curves { get { return curves; } set { curves = value; } }

        [AssetMember]
        public float[][] CurveKeys 
        { 
            get { return curveKeys; } 
            set 
            { 
                curveKeys = value;  
                if(curveKeys!=null)
                    keySamples = new SampleInfo[curveKeys.Length];
            }
        }

        [AssetMember]
        public int[] KeysIndices
        {
            get { return keysIndices; }
            set { keysIndices = value; }
        }

        public float LastKeyValue 
        {
            get 
            { 
                return lastKey < 0 ? lastKey = GetMaxKeyValue() : lastKey ; } 
        }

        [AssetMember(storeAs:StoreType.Reference)]
        public IAnimContext Context { get { return context; } set { context = value; } }              
       
        public void Sample(float time , float blendWeight = 1.0f , bool blended = false)
        {                   
            #region Compute Keys

            for (int i = 0; i < curveKeys.Length; i++)
            {              
                SampleInfo info;

                var keys = CurveKeys[i];
                if (keys.Length == 0)
                    continue;
                else if (keys.Length == 1)
                {
                    info.S = 0;
                    info.HightKey = 0;
                    info.LowKey = 0;
                }
                else
                {
                    info.LowKey = _FindKeys(time, keys);
                    info.HightKey = Math.Min(info.LowKey + 1, keys.Length - 1);

                    float k0 = keys[info.LowKey];
                    float k1 = keys[info.HightKey];
                    info.S = (time - k0) / (k1 - k0);
                }

                keySamples[i] = info;
            }

            #endregion

            #region Sample Curves
           
            for (int i = 0; i < curves.Length; i++)
            { 
                float[] keys;
                SampleInfo info;
                if (keysIndices != null)
                {
                    keys = curveKeys[keysIndices[i]];
                    info = keySamples[keysIndices[i]];
                }
                else
                {
                    keys = curveKeys[0];
                    info = keySamples[0];
                }

                curves[i].Sample(info.S, info.LowKey, info.HightKey, keys, context);
            }

            #endregion

            context.OnSample(blended, blendWeight);
        }

        public float GetMaxKeyValue()
        {
            float lastKeyValue = _GetLasKeyValue(curveKeys[0]);
            foreach (var keys in curveKeys)
            {
                lastKeyValue = Math.Max(lastKeyValue, _GetLasKeyValue(keys));
            }

            return lastKeyValue;
        }

        public float GetMinKeyValue()
        {
            float minKeyValue = curveKeys[0][0];
            foreach (var keys in curveKeys)
            {
                minKeyValue = Math.Min(minKeyValue, keys[0]);
            }

            return minKeyValue;
        }

        private float _GetLasKeyValue(float[] keys) { return keys[keys.Length - 1]; }

        public override string ToString()
        {
            return name ?? base.ToString();
        }

        public Asset CreateAsset()
        {
            return Asset.Create(this, name);
        }      

        private int _FindKeys(float time, float[] keys)
        {
            if (time <= keys[0])
                return 0;            

            int ini = 0;
            int end = keys.Length - 1;    
        
            while (true)
            {                
                int diff  = end - ini;
                if (diff == 0)
                {                    
                    if (time >= keys[ini])
                        return ini;
                    else
                        return ini - 1;                  
                }
                else if (diff == 1)
                {                 
                    return ini;                    
                }
                else
                {                    
                    int middle = (ini + end) >> 1;
                    if (time > keys[middle])
                        ini = middle;
                    else
                        end = middle;
                }

            }
        }        
       
        [Serializable]
        class Activator : IProviderActivator
        {
            AssetReference[] contexts;
            bool[] blended;          
            float lastKey;
            string name;

            public void Initialize(IAssetProvider provider)
            {
                var node = (AnimationNode)provider;
                lastKey = node.lastKey;              
                name = node.name;             
            }

            public IAssetProvider CreateInstance()
            {
                var node = new AnimationNode(name);             
                node.lastKey = lastKey;              
                return node;    
            }
        }                         

        struct SampleInfo
        {
            public int LowKey;
            public int HightKey;
            public float S;
        }
    }   

}
