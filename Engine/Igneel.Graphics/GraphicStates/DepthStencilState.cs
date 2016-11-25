using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct StencilOperations
    {
        public StencilOperation StencilFailOp;
        public StencilOperation StencilDepthFailOp;
        public StencilOperation StencilPassOp;
        public Comparison StencilFunc;

        public StencilOperations(
            StencilOperation stencilFailOp = StencilOperation.Keep,
            StencilOperation stencilDepthFailOp = StencilOperation.Keep,
            StencilOperation stencilPassOp = StencilOperation.Keep,
            Comparison stencilFunc = Comparison.Always)
        {
            StencilFailOp = stencilFailOp;
            StencilDepthFailOp = stencilDepthFailOp;
            StencilPassOp = stencilPassOp;
            StencilFunc = stencilFunc;
        }

        public StencilOperations(StencilOperations other)
        {
            unsafe
            {
                fixed (StencilOperations* p = &this)
                {
                    *p = other;
                }
            }
        }

        public void SetDefaults()
        {
            StencilFailOp = StencilOperation.Keep;
            StencilDepthFailOp = StencilOperation.Keep;
            StencilPassOp = StencilOperation.Keep;
            StencilFunc = Comparison.Always;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct DepthStencilStateDesc
    {
        public bool DepthEnable;
        public bool WriteEnable;
        public Comparison DepthFunc;
        public bool StencilEnable;
        public byte StencilReadMask;
        public byte StencilWriteMask;
        public byte StencilRef;
        public StencilOperations FrontFace;
        public StencilOperations BackFace;

        public DepthStencilStateDesc(
            bool depthEnable = true,
            bool writeEnable = true,
            Comparison depthFunc = Comparison.Less,
            bool stencilEnable = false,
            byte stencilReadMask = 0xFF,
            byte stencilWriteMask = 0xFF,
            byte stencilRef = 0,
            StencilOperations? frontFace = null,
            StencilOperations? backFace = null)
        {
            DepthEnable = depthEnable;
            WriteEnable = writeEnable;
            DepthFunc = depthFunc;
            StencilEnable = stencilEnable;
            StencilReadMask = stencilReadMask;
            StencilWriteMask = stencilWriteMask;
            StencilRef = 0;
            FrontFace = frontFace ?? new StencilOperations(StencilOperation.Keep, StencilOperation.Keep, StencilOperation.Keep, Comparison.Always);
            BackFace = backFace ?? new StencilOperations(StencilOperation.Keep, StencilOperation.Keep, StencilOperation.Keep, Comparison.Always);
        }

        public DepthStencilStateDesc(DepthStencilStateDesc other)
        {
            unsafe
            {
                fixed (DepthStencilStateDesc* p = &this)
                {
                    *p = other;
                }
            }
        }

        public DepthStencilStateDesc(bool setdefault)
        {
            DepthEnable = true;
            WriteEnable = true;
            DepthFunc = Comparison.Less;
            StencilEnable = false;
            StencilReadMask = 0xFF;
            StencilWriteMask = 0xFF;
            FrontFace = new StencilOperations();
            BackFace = new StencilOperations();
            FrontFace.SetDefaults();
            BackFace.SetDefaults();
            StencilRef = 0;
        }

        public void SetDefaults()
        {
            DepthEnable = true;
            WriteEnable = true;
            DepthFunc = Comparison.Less;
            StencilEnable = false;
            StencilReadMask = 0xFF;
            StencilWriteMask = 0xFF;
            FrontFace = new StencilOperations();
            BackFace = new StencilOperations();
            FrontFace.SetDefaults();
            BackFace.SetDefaults();
            StencilRef = 0;
        }

        public static DepthStencilStateDesc Default
        {
            get
            {
                DepthStencilStateDesc d = new DepthStencilStateDesc();
                d.SetDefaults();
                return d;
            }
        }
    }

    public abstract class DepthStencilState : PipelineState<DepthStencilStateDesc>
    {
        public DepthStencilState(DepthStencilStateDesc state)
            : base(state)
        {

        }

        public byte StencilRef { get { return _state.StencilRef; } set { _state.StencilRef = value; } }
    }
}
