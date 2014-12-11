using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public enum IAFormat : byte
    {
        Color = 4,
        Dec3N = 14,
        Float1 = 0,
        Float2 = 1,
        Float3 = 2,
        Float4 = 3,
        HalfFour = 0x10,
        HalfTwo = 15,
        Short2 = 6,
        Short2N = 9,
        Short4 = 7,
        Short4N = 10,
        Ubyte4 = 5,
        UByte4N = 8,
        UDec3 = 13,
        Unused = 0x11,
        UShort2N = 11,
        UShort4N = 12
    }

    public enum IASemantic : byte
    {
        Binormal = 7,
        BlendIndices = 2,
        BlendWeight = 1,
        Color = 10,
        Depth = 12,
        Fog = 11,
        Normal = 3,
        PointSize = 4,
        Position = 0,
        PositionTransformed = 9,
        Sample = 13,
        Tangent = 6,
        TessellateFactor = 8,
        TextureCoordinate = 5
    }

     [Flags]
    public enum IAPrimitive
    {
        PointList = 1,
        LineList = 2,
        LineStrip = 3,
        TriangleList = 4,
        TriangleStrip = 5,
        TriangleFan = 6
    }

    [Flags]
    public enum ResourceUsage
    {
        /// <summary>
        /// A resource that requires read and write access by the GPU. This is likely to be the most common usage choice. 
        /// </summary>
        Default = 0,

        /// <summary>
        /// A resource that can only be read by the GPU. It cannot be written by the GPU, 
        /// and cannot be accessed at all by the CPU. This type of resource must be initialized when it is created, 
        /// since it cannot be changed after creation. 
        /// </summary>
        Immutable = 1,

        /// <summary>
        /// A resource that is accessible by both the GPU and the CPU (write only). 
        /// A dynamic resource is a good choice for a resource that will be updated by the CPU at least once per frame
        /// </summary>
        Dynamic = 2,

        /// <summary>
        /// A resource that supports data transfer (copy) from the GPU to the CPU
        /// </summary>
        Staging = 3
    }

    [Flags]
    public enum MapType
    {
        /// <summary>
        /// Resource is mapped for reading
        /// </summary>
        Read = 1,
        /// <summary>
        /// Resource is mapped for writing
        /// </summary>
        Write = 2,
        /// <summary>
        /// Resource is mapped for reading and writing
        /// </summary>
        ReadWrite = 3,

        /// <summary>
        /// Resource is mapped for writing; the previous contents of the resource will be undefined
        /// </summary>
        Write_Discard = 4,

        /// <summary>
        /// Resource is mapped for writing; the existing contents of the resource cannot be overwritten
        /// </summary>
        Write_No_OverWrite = 5
    }

    public enum IndexFormat
    {
        /// <summary>
        /// 16-bit  index buffer
        /// </summary>
        Index16 = 0,
        /// <summary>
        /// 32-bit   index buffer
        /// </summary>
        Index32 = 0x1
    }

    [Flags]
    public enum ResourceOptionFlags
    {
        GdiCompatible = 0x20,
        GenerateMipMaps = 1,
        KeyedMutex = 0x10,
        None = 0,
        Shared = 2,
        TextureCube = 4
    }


}
