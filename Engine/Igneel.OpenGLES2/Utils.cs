using Igneel.Graphics;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.OpenGLES2
{
    public static class Utils
    {
        public static void GetFormat(Format format, out All glClientFormat, out All glType, out All interalFormat)
        {
            switch (format)
            {
                case Format.B8G8R8A8_UNORM_SRGB:
                case Format.B8G8R8A8_UNORM:
                    glType = All.UnsignedByte;
                    glClientFormat = All.BgraExt;
                    interalFormat = All.Rgba;
                    break;

                case Format.R8G8B8A8_UNORM_SRGB:
                case Format.R8G8B8A8_UNORM:
                    glType = All.UnsignedByte;
                    glClientFormat = All.Rgba;
                    interalFormat = All.Rgba;
                    break;


                case Format.R16G16B16A16_UINT:
                case Format.R16G16B16A16_UNORM:
                    glType = All.UnsignedShort;
                    glClientFormat = All.Rgba;
                    interalFormat = All.Rgba;
                    break;
                case Format.R16G16B16A16_SINT:
                    glType = All.Short;
                    glClientFormat = All.Rgba;
                    interalFormat = All.Rgba;
                    break;
                case Format.R16G16B16A16_FLOAT:
                    glType = All.HalfFloatOes;
                    glClientFormat = All.Rgba;
                    interalFormat = All.Rgba;
                    break;


                case Format.R32G32B32A32_FLOAT:
                    glType = All.Float;
                    glClientFormat = All.Rgba;
                    interalFormat = All.Rgba;
                    break;
                case Format.R32G32B32A32_SINT:
                    glType = All.Int;
                    glClientFormat = All.Rgba;
                    interalFormat = All.Rgba;
                    break;

                case Format.R32G32B32_UINT:
                    glType = All.UnsignedInt;
                    glClientFormat = All.Rgb;
                    interalFormat = All.Rgb;
                    break;
                case Format.R32G32B32_FLOAT:
                    glType = All.Float;
                    glClientFormat = All.Rgb;
                    interalFormat = All.Rgb;
                    break;
                case Format.R32G32B32_SINT:
                    glType = All.Int;
                    glClientFormat = All.Rgb;
                    interalFormat = All.Rgb;
                    break;


                case Format.R32_FLOAT:
                    glType = All.Float;
                    glClientFormat = All.RedBits;
                    interalFormat = All.Luminance;
                    break;

                case Format.R32_UINT:
                    glType = All.UnsignedInt;
                    glClientFormat = All.RedBits;
                    interalFormat = All.Luminance;
                    break;
                case Format.R32_SINT:
                    glType = All.Int;
                    glClientFormat = All.RedBits;
                    interalFormat = All.Luminance;
                    break;

                case Format.D16_UNORM:
                    glType = All.Short;
                    glClientFormat = (All)PixelFormat.DepthComponent;
                    interalFormat = (All)PixelFormat.DepthComponent;
                    break;
                case Format.D24_UNORM_S8_UINT:
                    glType = All.Int;
                    glClientFormat = (All)PixelFormat.DepthComponent;
                    interalFormat = (All)PixelFormat.DepthComponent;
                    break;
                case Format.D32_FLOAT:
                    glType = All.Float;
                    glClientFormat = (All)PixelFormat.DepthComponent;
                    interalFormat = (All)PixelFormat.DepthComponent;
                    break;
                default:
                    throw new NotImplementedException();

            }
        }

        public static int GetSize(All glType)
        {
            switch (glType)
            {
                case All.Byte:
                case All.UnsignedByte: return 1;
                case All.Float:
                case All.UnsignedInt:
                case All.Int: return 8;
                case All.HalfFloatOes: return 2;
                default:
                    throw new NotImplementedException();
            }
        }

        public static int GetElements(All glFormat)
        {
            switch (glFormat)
            {
                case All.Rgb: return 3;
                case All.Rgba: return 4;
                case All.LuminanceAlpha:
                case All.Luminance: return 1;
                default:
                    throw new NotImplementedException();
            }
        }

        public static All GetTextureCubeFace(int resource)
        {
            switch (resource)
            {
                case 0: return All.TextureCubeMapPositiveX;
                case 1: return All.TextureCubeMapNegativeX;
                case 2: return All.TextureCubeMapPositiveY;
                case 3: return All.TextureCubeMapNegativeY;
                case 4: return All.TextureCubeMapPositiveZ;
                case 5: return All.TextureCubeMapNegativeZ;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static All GetUsage(ResourceUsage usage)
        {
            switch (usage)
            {
                case ResourceUsage.Default:
                    return All.StaticDraw;
                case ResourceUsage.Immutable:
                    return All.StaticDraw;
                case ResourceUsage.Dynamic:
                    return All.DynamicDraw;
                case ResourceUsage.Staging:
                    return All.Zero;
                default:
                    return All.Zero;
            }
        }

        [SuppressUnmanagedCodeSecurity, DllImport("libGLESv2.dll", EntryPoint = "glTexImage2D", ExactSpelling = true)]
        public static extern void TexImage2D(All target, int level, int internalformat, int width, int height, int border, All format, All type, IntPtr pixels);

        [SuppressUnmanagedCodeSecurity, DllImport("libGLESv2.dll", EntryPoint = "glGetBufferPointervOES", ExactSpelling = true)]
        public static extern void GetBufferPointervOES(All target, All pname, [Out] IntPtr @params);

        public static int GetElements(IAFormat iAFormat)
        {
            switch (iAFormat)
            {
                case IAFormat.Color: return 4;             
                case IAFormat.Float1: return 1;
                case IAFormat.Float2: return 2;
                case IAFormat.Float3: return 3;
                case IAFormat.Float4: return 4;
                case IAFormat.HalfFour: return 4;
                case IAFormat.HalfTwo:
                case IAFormat.Short2:
                case IAFormat.Short2N: return 2;
                case IAFormat.Short4: return 4;
                case IAFormat.Short4N: return 4;
                case IAFormat.Ubyte4: return 4;
                case IAFormat.UByte4N: return 4;                
                case IAFormat.Unused: throw new ArgumentException();
                case IAFormat.UShort2N: return 2;
                case IAFormat.UShort4N: return 4;
                default:
                    throw new ArgumentException();
            }
        }

        public static int GetSize(IAFormat iAFormat)
        {
            switch (iAFormat)
            {
                case IAFormat.Ubyte4: 
                case IAFormat.UByte4N: 
                case IAFormat.Color: return 4;            
                case IAFormat.Float1: return 4;
                case IAFormat.Float2: return 8;
                case IAFormat.Float3: return 12;
                case IAFormat.Float4: return 16;
                case IAFormat.HalfFour: return 8;
                case IAFormat.HalfTwo:
                case IAFormat.UShort2N:
                case IAFormat.Short2:
                case IAFormat.Short2N: return 4;
                case IAFormat.UShort4N:
                case IAFormat.Short4: 
                case IAFormat.Short4N: return 8;                
                case IAFormat.Unused: throw new ArgumentException();                                 

                default:
                    throw new ArgumentException();
            }
        }
     
        public static All GetType(IAFormat iAFormat)
        {
            switch (iAFormat)
            {
                case IAFormat.Color: return All.UnsignedByte;
                case IAFormat.Dec3N: return All.Int;
                case IAFormat.Float1: 
                case IAFormat.Float2: 
                case IAFormat.Float3:
                case IAFormat.Float4: return All.Float;
                case IAFormat.HalfFour:
                case IAFormat.HalfTwo: return All.HalfFloatOes;
                case IAFormat.Short2: return All.Short;
                case IAFormat.Short2N: return All.UnsignedShort;
                case IAFormat.Short4:
                case IAFormat.Short4N: return All.Short;
                case IAFormat.Ubyte4: return All.UnsignedByte;
                case IAFormat.UByte4N: return All.UnsignedByte;
                case IAFormat.UDec3: return All.Int;
                case IAFormat.Unused: throw new ArgumentException();
                case IAFormat.UShort2N: return All.UnsignedShort;
                case IAFormat.UShort4N: return All.UnsignedShort;
                default:
                    throw new ArgumentException();
            }
        }
       
        public static bool GetNormalized(IAFormat iAFormat)
        {
            switch (iAFormat)
            {                
                case IAFormat.Dec3N:
                case IAFormat.Short2N:
                case IAFormat.Short4N:
                case IAFormat.UByte4N:
                case IAFormat.UShort2N:
                case IAFormat.UShort4N:
                case IAFormat.Color: return true;

                case IAFormat.Float1:
                case IAFormat.Float2:
                case IAFormat.Float3:
                case IAFormat.Float4:
                case IAFormat.HalfFour:
                case IAFormat.HalfTwo:
                case IAFormat.Short2: 
                case IAFormat.Short4:                
                case IAFormat.Ubyte4:                 
                case IAFormat.UDec3:
                    return false;
                case IAFormat.Unused: throw new ArgumentException();                
                default:
                    throw new ArgumentException();
            }
        }
    }
}
