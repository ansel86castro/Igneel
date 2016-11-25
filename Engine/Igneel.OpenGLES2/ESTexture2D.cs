using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Graphics;
using OpenTK.Graphics.ES20;
using System.Runtime.InteropServices;

namespace Igneel.OpenGLES2
{
    public class ESTexture2D:Texture2DBase
    {
        public const int PACK_ALIGNMENT = 4;
        int texture;
        All glClientFormat;
        All glType;

        byte[] stagingBuffer;
        GCHandle handle;
        public ESTexture2D(Texture2DDesc desc, MappedTexture2D[] data)
            : base(desc)
        {        
            All glInternalFormat;
            Utils.GetFormat(desc.Format, out glClientFormat, out glType, out glInternalFormat);
            if (desc.Usage == ResourceUsage.Default || desc.Usage == ResourceUsage.Immutable)
            {
                texture = GL.GenTexture();
                All texType;
                if (desc.ArraySize == 1)
                {
                    texType = All.Texture2D;
                }
                else if (desc.ArraySize == 6 && desc.Options == ResourceOptionFlags.TextureCube)
                    texType = All.TextureCubeMap;
                else
                    throw new ArgumentException("desc");

                GL.BindTexture(texType, texture);
                if (texType == All.Texture2D)
                    GL.TexImage2D(texType, 0, (int)glInternalFormat, desc.Width, desc.Height, 0, glClientFormat, glType, data[0].DataPointer);
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        IntPtr pdata = data != null ? data[i].DataPointer : IntPtr.Zero;
                        GL.TexImage2D(Utils.GetTextureCubeFace(i), 0, (int)glInternalFormat, desc.Width, desc.Height, 0, glClientFormat, glType, pdata);    
                    }                  
                }

                GL.TexParameter(texType, All.TextureMinFilter, (int)All.Linear);
                GL.TexParameter(texType, All.TextureMagFilter, (int)All.Linear);
                GL.TexParameter(texType, All.TextureWrapS, (int)All.ClampToEdge);
                GL.TexParameter(texType, All.TextureWrapT, (int)All.ClampToEdge);

                GL.BindTexture(texType, 0);
            }
            else
            {
                int rowSize = Width * Utils.GetSize(glType) * Utils.GetElements(glClientFormat);
                stagingBuffer = new byte[rowSize * desc.Height];

                if (data!=null)
                    ClrRuntime.Runtime.Copy(data[0].DataPointer, stagingBuffer, 0, stagingBuffer.Length);
            }
            
        }

        public override MappedTexture2D Map(int subResource, MapType map, bool doNotWait)
        {
            MappedTexture2D m = new MappedTexture2D();
            int rowSize = Width * Utils.GetSize(glType) * Utils.GetElements(glClientFormat);
            m.RowPitch = rowSize + rowSize % PACK_ALIGNMENT;
            
            if (stagingBuffer == null)
            {               
                if (Description.Options == ResourceOptionFlags.TextureCube)
                {
                    int arrayIndex;
                    int level;
                    Texture2DBase.DecoupleSubresource(subResource, MipLevels, out arrayIndex, out level);
                    GL.BindTexture(All.TextureCubeMap, texture);
                    GL.ExtGetTexSubImageQCOM(Utils.GetTextureCubeFace(arrayIndex), level, 0, 0, 0, Width, Height, 0, glClientFormat, glType, m.DataPointer);
                }
                else
                {
                    GL.BindTexture(All.Texture2D, texture);
                    GL.ExtGetTexSubImageQCOM(All.Texture2D, subResource, 0, 0, 0, Width, Height, 0, glClientFormat, glType, m.DataPointer);
                }
            }
            else
            {
                handle = GCHandle.Alloc(stagingBuffer, GCHandleType.Pinned);
                 m.DataPointer = ClrRuntime.Runtime.GetPtr(stagingBuffer, 0);
            }
            return m;
        }

        public override void UnMap(int subResource)
        {
            if (stagingBuffer == null)
            {
                if (Description.Options == ResourceOptionFlags.TextureCube)
                    GL.BindTexture(All.TextureCubeMap, 0);
                else
                    GL.BindTexture(All.Texture2D, 0);
            }
            else
            {
                handle.Free();
            }
        }        
    }
}
