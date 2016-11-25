using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public enum Filter:uint
    {
        Anisotropic = 0x55,
        ComparisonAnisotropic = 0xd5,
        ComparisonMinLinearMagMipPoint = 0x90,
        ComparisonMinLinearMagPointMipLinear = 0x91,
        ComparisonMinMagLinearMipPoint = 0x94,
        ComparisonMinMagMipLinear = 0x95,
        ComparisonMinMagMipPoint = 0x80,
        ComparisonMinMagPointMipLinear = 0x81,
        ComparisonMinPointMagLinearMipPoint = 0x84,
        ComparisonMinPointMagMipLinear = 0x85,
        MinLinearMagMipPoint = 0x10,
        MinLinearMagPointMipLinear = 0x11,
        MinMagLinearMipPoint = 20,
        MinMagMipLinear = 0x15,
        MinMagMipPoint = 0,
        MinMagPointMipLinear = 1,
        MinPointMagLinearMipPoint = 4,
        MinPointMagMipLinear = 5,
        Texture1Bit = 0x80000000
    }

    public enum TextureAddressMode
    {
        Border = 4,
        Clamp = 3,
        Mirror = 2,
        MirrorOnce = 5,
        Wrap = 1
    }

    [Flags]
    public enum FormatSupport
    {
        BlendOperation = 0x8000,
        Buffer = 1,
        CpuLocking = 0x20000,
        DepthStencil = 0x10000,
        FormatCastSupport = 0x100000,
        FormatDisplaySupport = 0x80000,
        FormatMultisampleLoadSupport = 0x400000,
        FormatMultisampleRenderTargetSupport = 0x200000,
        IndexBuffer = 4,
        MipMap = 0x1000,
        MipMapAutoGeneration = 0x2000,
        MultisampleResolve = 0x40000,
        None = 0,
        RenderTarget = 0x4000,
        ShaderLoadIntrinsic = 0x100,
        ShaderSampleComparisonIntrinsic = 0x400,
        ShaderSampleIntrinsic = 0x200,
        StreamOutputBuffer = 8,
        Texture1D = 0x10,
        Texture2D = 0x20,
        Texture3D = 0x40,
        TextureCube = 0x80,
        VertexBuffer = 2
    }

    public enum GraphicDeviceType
    {
        Hardware = 1,
        NullReference = 4,
        Reference = 2,
        Software = 3
    }

    public enum VertexProcessing
    {
        Hardware = 1,
        Software = 3
    }

    [Flags]
    public enum ClearFlags
    {
        All = 7,
        None = 0,
        Stencil = 4,
        Target = 1,
        ZBuffer = 2
    }

    public enum PresentionInterval:long
    {
        Default = 0x00000000L,
        One = 0x00000001L,
        Two = 0x00000002L,
        Three = 0x00000004L,
        Four = 0x00000008L,       
    }

}
