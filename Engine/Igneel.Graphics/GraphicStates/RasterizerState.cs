using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
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
        public bool FrontCounterClockwise;

        public RasterizerDesc(
            FillMode fill = FillMode.Solid,
            CullMode cull = CullMode.Back,
            int depthBias = 0,
            float depthBiasClamp = 0,
            float slopeScaledDepthBias = 0,
            bool depthClipEnable = true,
            bool scissorEnable = false,
            bool multisampleEnable = false,
            bool antialiasedLineEnable = false,
            bool frontCounterClockwise=false)
        {
            Fill = fill;
            Cull = cull;
            DepthBias = depthBias;
            DepthBiasClamp = depthBiasClamp;
            SlopeScaledDepthBias = slopeScaledDepthBias;
            DepthClipEnable = depthClipEnable;
            ScissorEnable = scissorEnable;
            MultisampleEnable = multisampleEnable;
            AntialiasedLineEnable = antialiasedLineEnable;
            FrontCounterClockwise = frontCounterClockwise;
        }

        public RasterizerDesc(
          FillMode fill = FillMode.Solid,
          CullMode cull = CullMode.Back,
          int depthBias = 0,
          float depthBiasClamp = 0,
          float slopeScaledDepthBias = 0,
          bool depthClipEnable = true,
          bool scissorEnable = false,
          bool multisampleEnable = false,
          bool antialiasedLineEnable = false)
            : this(fill, cull, depthBias, depthBiasClamp, slopeScaledDepthBias, depthClipEnable, scissorEnable, multisampleEnable, antialiasedLineEnable, false)
        {

        }

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
            FrontCounterClockwise = false;
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
            FrontCounterClockwise = false;
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

    public abstract class RasterizerState : PipelineState<RasterizerDesc>
    {       
        public RasterizerState(RasterizerDesc state)
            : base(state)
        {

        }             
    }
}
