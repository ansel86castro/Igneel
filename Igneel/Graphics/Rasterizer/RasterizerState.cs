using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{

    public struct RasterizerDesc
    {
        public FillMode Fill;
        public CullMode Cull;
        public int DepthBias;
        public float DepthBiasClamp;
        public float SlopeScaledDepthBias;
        public bool DepthClipEnable;
        public bool ScissorEnable;
        public bool MultisampleEnable;
        public bool AntialiasedLineEnable;

        public RasterizerDesc(bool setDefaults)
        {
            Fill = FillMode.Solid;
            Cull = CullMode.Back;
            DepthBias = 0;
            DepthBiasClamp = 0;
            SlopeScaledDepthBias = 0;
            DepthClipEnable = true;
            ScissorEnable = false;
            MultisampleEnable = false;
            AntialiasedLineEnable = false;
        }

        public RasterizerDesc(RasterizerDesc other)
        {
            unsafe
            {
                fixed (RasterizerDesc* p = &this)
                {
                    *p = other;
                }
            }
        }

        public void SetDefaults()
        {
            Fill = FillMode.Solid;
            Cull = CullMode.Back;
            DepthBias = 0;
            DepthBiasClamp = 0;
            SlopeScaledDepthBias = 0;
            DepthClipEnable = true;
            ScissorEnable = false;
            MultisampleEnable = false;
            AntialiasedLineEnable = false;
        }

        public RasterizerDesc Default
        {
            get
            {
                var d = new RasterizerDesc();
                d.SetDefaults();
                return d;
            }
        }
    }

    public abstract class RasterizerState : GraphicDeviceState<RasterizerDesc>
    {       
        public RasterizerState(RasterizerDesc state)
            : base(state)
        {

        }             
    }
}
