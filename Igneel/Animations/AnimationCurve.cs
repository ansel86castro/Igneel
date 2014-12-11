using ClrPlatform;
using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Animations
{
    public enum InterpolationMethod
    {
        LINEAR,
        BEZIER,
        CARDINAL,
        HERMITE,
        BSPLINE,
        STEP,
        QUATSLERP,
        QUATSLERPX,
        QUATSLERPY,
        QUATSLERPZ,
    }  

    /// <summary>    
    /// Define a animation curve for a property (channel)
    /// </summary>
    public class AnimationCurve:INameable, IAssetProvider
    {        
        ICurveOutput channel;
        float[] output;
        int outDim = 1;
        float[] in_tangent;
        float[] out_tangent;
        InterpolationMethod interpolationType;               
        float[] result = new float[1];

        [AssetMember]
        public string Name { get; set; }

        [AssetMember(storeAs: StoreType.Reference)]
        public ICurveOutput CurveOutput
        {
            get { return channel; }
            set { channel = value; }
        }       

        [AssetMember]
        public float[] Output
        {
            get { return output; }
            set { output = value; }
        }

        [AssetMember]
        public int OutputDim 
        { 
            get { return outDim; }
            set 
            { 
                outDim = value;
                result = new float[outDim];
            } 
        }

        [AssetMember]
        public float[] In_tangent
        {
            get { return in_tangent; }
            set { in_tangent = value; }
        }

        [AssetMember]
        public float[] Out_tangent
        {
            get { return out_tangent; }
            set { out_tangent = value; }
        }

        [AssetMember]
        public InterpolationMethod InterpolationType
        {
            get { return interpolationType; }
            set { interpolationType = value; }
        }
      
        public AnimationCurve()
        {
            
        }

        //private float SampleArray(float[] array, int index)
        //{
        //    if (array == null)
        //        return 0;
        //    return array.Length == 1 ? array[0] : array[index];
        //}

        public void Sample(float s, int lowkey, int hightKey, float[] keys, IAnimContext context)
        {
            if (output.Length > 1 && (lowkey >= output.Length || hightKey >= output.Length))
                return;
            unsafe
            {
                if (outDim == 1)
                {
                    float v;
                    if (output.Length == 1)
                        v = output[0];
                    else
                    {
                        float p0 = output[lowkey];
                        float p1 = output[hightKey];
                        float t0 = out_tangent == null ? 0 : (out_tangent.Length == 1 ? out_tangent[0] : out_tangent[lowkey]);
                        float t1 = in_tangent == null ? 0 : (in_tangent.Length == 1 ? in_tangent[0] : in_tangent[hightKey]);
                        switch (interpolationType)
                        {
                            case InterpolationMethod.STEP:
                                v = p0;
                                break;
                            case InterpolationMethod.LINEAR:
                                v = Numerics.Lerp(p0, p1, s);
                                break;
                            case InterpolationMethod.BEZIER:
                                //v = Numerics.Bezier(p0, p1, t0, t1, s);
                                v = Numerics.Bezier(p0, p1, t0 / 3.0f + p0, p1 - t1 / 3.0f , s);                                
                                break;
                            case InterpolationMethod.HERMITE:
                                v = Numerics.Hermite(p0, p1, t0, t1, s);
                                break;
                            case InterpolationMethod.QUATSLERPX:
                                v = Quaternion.Slerp(new Quaternion(1, 0, 0, p0), new Quaternion(1, 0, 0, p1), s).W;
                                break;
                            case InterpolationMethod.QUATSLERPY:
                                v = Quaternion.Slerp(new Quaternion(0, 1, 0, p0), new Quaternion(0, 1, 0, p1), s).W;
                                break;
                            case InterpolationMethod.QUATSLERPZ:
                                v = Quaternion.Slerp(new Quaternion(0, 0, 1, p0), new Quaternion(0, 0, 1, p1), s).W;
                                break;
                            default:
                                throw new InvalidOperationException("Interpolation method not implemented");
                        }
                    }
                    channel.SetOutput((IntPtr)(&v), context);

                }
                else
                {
                    fixed (float* pResult = result)
                    {
                        fixed (float* pOutput = output)
                        {
                            if (output.Length == outDim)
                                Crl.CopyMemory(pOutput, pResult, outDim * sizeof(float));
                            else
                            {
                                float* p0 = pOutput + lowkey * outDim;
                                float* p1 = pOutput + hightKey * outDim;
                                float t0 = out_tangent == null ? 0 : (out_tangent.Length == 1 ? out_tangent[0] : out_tangent[lowkey]);
                                float t1 = in_tangent == null ? 0 : (in_tangent.Length == 1 ? in_tangent[0] : in_tangent[hightKey]);
                                switch (interpolationType)
                                {
                                    case InterpolationMethod.STEP:
                                        Crl.CopyMemory(p0, pResult, outDim * sizeof(float));
                                        break;
                                    case InterpolationMethod.LINEAR:
                                        Numerics.Lerp(pResult, p0, p1, s, outDim);
                                        break;
                                    case InterpolationMethod.BEZIER:
                                        Numerics.Bezier(p0, p1, t0, t1, s, pResult, outDim);                                        
                                        break;
                                    case InterpolationMethod.HERMITE:
                                        Numerics.Hermite(p0, p1, t0, t1, s, pResult, outDim);
                                        break;
                                    case InterpolationMethod.QUATSLERP:
                                        *(Quaternion*)pResult = Quaternion.Slerp(*(Quaternion*)p0, *(Quaternion*)p1, s);
                                        break;
                                    default:
                                        throw new InvalidOperationException("Interpolation method not implemented");
                                }
                            }
                            channel.SetOutput((IntPtr)pResult, context);
                        }
                    }
                }
            }
        }      

        public override string ToString()
        {
            if (Name != null)
                return Name;
            return base.ToString();
        }

        public Asset CreateAsset()
        {
            return Asset.Create(this, Name);
        }
    }
}
