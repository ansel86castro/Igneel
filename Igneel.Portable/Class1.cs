using System;
using System.Collections.Generic;
using System.Text;

namespace Igneel.Portable
{
    public class Class1
    {
		Vertex vertex;
        public struct Vertex
        {
            public float x, y, z;
        }

        public Class1()
        {
            unsafe
            {
                Vertex v1 = new Vertex();
				Vertex v2 = new Vertex(){ x = 10 };
                Vertex* pVertex = &v1;
                *pVertex = v2;
				this.vertex = *pVertex;        
                
            }            
        }
    }
}
