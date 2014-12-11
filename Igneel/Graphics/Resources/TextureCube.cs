using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    //public abstract class TextureCube : Texture
    //{
    //    /// <summary>
    //    /// Texture width (in texels). 
    //    /// </summary>
    //    protected int _edgeSize;


    //    protected TextureCubeDesc _desc = new TextureCubeDesc();

    //    public TextureCube()
    //    {
    //        _type = ResourceType.TextureCube;
    //    }

    //    /// <summary>
    //    /// Texture width (in texels). 
    //    /// </summary>
    //    public int EdgeSize { get { return _edgeSize; } }

    //    public Format Format { get { return _desc.Format; } }

    //    public TextureCubeDesc Description { get { return _desc; } }

    //    public abstract MappedTexture2D Map(int face, int subResource, MapType map, bool doNotWait);

    //    public abstract void UnMap(int face , int subResource);

    //    public abstract SubResource GetSubResource(int face, int level);

    //    public static implicit operator TextureCube(string filename)
    //    {
    //        TextureCube tex = (TextureCube)Engine.Graphics.CreateTextureFromFile(ResourceType.Texture3D, filename);
    //        tex._filename = filename;
    //        return tex;
    //    }
    //}
}
