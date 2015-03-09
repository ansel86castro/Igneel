using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public struct SamplerDesc
    {
        public Filter Filter;
        public TextureAddressMode AddressU;
        public TextureAddressMode AddressV;
        public TextureAddressMode AddressW;
        public float MipLODBias;
        public int MaxAnisotropy;
        public Comparison ComparisonFunc;
        public Color4 BorderColor;
        public float MinLOD;
        public float MaxLOD;

        public SamplerDesc(SamplerDesc other)
        {
            unsafe
            {
                fixed (SamplerDesc* p = &this)
                {
                    *p = other;
                }
            }
        }

        public SamplerDesc(bool setDefaults)
        {
            AddressU = TextureAddressMode.Wrap;
            AddressV = TextureAddressMode.Wrap;
            AddressW = TextureAddressMode.Wrap;
            BorderColor = new Color4(0);
            Filter = Graphics.Filter.MinMagMipLinear;
            MipLODBias = 0;
            MaxAnisotropy = 1;
            ComparisonFunc = Comparison.Never;
            MinLOD = 0;
            MaxLOD = 0;
        }

        public void SetDefaults()
        {
            AddressU = TextureAddressMode.Wrap;
            AddressV = TextureAddressMode.Wrap;
            AddressW = TextureAddressMode.Wrap;
            BorderColor = new Color4(0);
            Filter = Graphics.Filter.MinMagMipLinear;
            MipLODBias = 0;
            MaxAnisotropy = 1;
            ComparisonFunc = Comparison.Never;
            MinLOD = 0;
            MaxLOD = 0;
        }

        public static SamplerDesc GetDefaults()
        {
            SamplerDesc d = new SamplerDesc();
            d.SetDefaults();
            return d;
        }

    }

    public abstract class SamplerState : PipelineState<SamplerDesc>
    {
        static SamplerState linearSample;
        static SamplerState pointSample;

        public SamplerState(SamplerDesc desc)
            : base(desc)
        {

        }

        public static SamplerState Linear
        {
            get
            {
                if (linearSample == null)
                {
                    var desc = new SamplerDesc(true);
                    desc.Filter = Filter.MinPointMagMipLinear;
                    desc.AddressU = TextureAddressMode.Wrap;
                    desc.AddressV = TextureAddressMode.Wrap;
                     desc.AddressW = TextureAddressMode.Wrap;
                    linearSample = Engine.Graphics.CreateSamplerState(desc);
                }

                return linearSample;
            }
        }

        public static SamplerState Point
        {
            get
            {
                if (pointSample == null)
                {
                    pointSample = Engine.Graphics.CreateSamplerState(new SamplerDesc(true)
                    {
                        AddressU = TextureAddressMode.Border,
                        AddressV = TextureAddressMode.Border,
                        AddressW = TextureAddressMode.Border,
                        BorderColor = new Color4(0),
                        Filter = Filter.MinMagMipPoint
                    });
                }
                return pointSample;

            }
        }
    }
}
