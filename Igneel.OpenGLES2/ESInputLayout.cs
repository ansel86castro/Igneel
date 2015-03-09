using Igneel.Graphics;
using OpenTK.Graphics.ES20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.OpenGLES2
{
    public class ESInputLayout: InputLayout
    {
        private VertexElement[] elements;
        public ESInputLayout(VertexElement[] elements)
        {
            this.elements = elements;
        }

        public void SetupBuffers(ESBuffer vertexBuffer, int stream)
        {
            GL.BindBuffer(All.ArrayBuffer, vertexBuffer.buffer);           
            for (int i = 0; i < elements.Length; i++)
            {
                var e = elements[i];
                if (e.Stream == stream)
                {
                    GL.EnableVertexAttribArray(i);
                    GL.VertexAttribPointer(i, Utils.GetElements(e.Format), Utils.GetType(e.Format), Utils.GetNormalized(e.Format), Utils.GetSize(e.Format), new IntPtr(e.Offset));
                }
            }
        }

        public void DisableBuffers(ESBuffer vertexBuffer, int stream)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                var e = elements[i];
                if (e.Stream == stream)
                {
                    GL.DisableVertexAttribArray(i);                    
                }
            }
        }
        public void DisableAllAttribs()
        {
            for (int i = 0; i < elements.Length; i++)
            {
                GL.DisableVertexAttribArray(i);
            }
        }
    }
}
