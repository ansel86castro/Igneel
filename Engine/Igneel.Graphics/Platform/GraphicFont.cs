using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{

    public enum FontStyle { Normal, Italic, Bold }

    public abstract class GraphicFont:ResourceAllocator
    {
        private Texture2D texture;
        private GraphicDevice device;

        public GraphicFont(GraphicDevice device)
        {
            this.device = device;
        }

        public string Family { get; protected set; }

        public int Size { get; protected set; }

        public int FontWidth { get; protected set; }

        public int FontHeight { get; protected set; }

        public FontStyle Style { get; set; }

        public Texture2D FontTexture { get { return texture; } }

        public GraphicDevice Device { get { return device; } }

        protected void Init()
        {
            DrawFontToBitmap();
            MappedTexture2D mt = LockFontBitmap(Format.R8G8B8A8_UNORM);

            texture = device.CreateTexture2D(new Texture2DDesc
            {
                 ArraySize = 1,
                 BindFlags = BindFlags.ShaderResource,
                 CPUAccessFlags = CpuAccessFlags.None,
                 Format = Format.R8G8B8A8_UNORM,
                
            }, new MappedTexture2D[] { mt });

            UnlockFontBitmap();
            DisposeBitmap();
        }

        protected abstract void OnSizeChanged();

        protected  abstract void DrawFontToBitmap();

        protected abstract void DisposeBitmap();

        protected abstract MappedTexture2D LockFontBitmap(Format format);

        protected abstract void UnlockFontBitmap();

        public void DrawText(string text, Rectangle rec, Color4 color, DrawFontFormat format)
        {

        }

    }
}
