﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Igneel.Design;

namespace Igneel.Components
{
    [Serializable]
    [TypeConverter(typeof(DesignTypeConverter))]
    public class MeshPart
    {
        internal int materialIndex = -1;
        internal int layerID = -1;        
        internal int startIndex;
        internal int primitiveCount;
        internal int startVertex;
        internal int vertexCount;
        internal int indexCount;  

        public MeshPart(int startIndex, int primitiveCount, int startVertex, int vertexCount)
        {
            this.startIndex = startIndex;
            this.primitiveCount = primitiveCount;
            this.startVertex = startVertex;
            this.vertexCount = vertexCount;
            this.indexCount = primitiveCount * 3;
        }

        public MeshPart(int startIndex, int primitiveCount, int startVertex, int vertexCount, int materialIndex)
            :this(startIndex,primitiveCount,startVertex, vertexCount)
        {
            this.materialIndex = materialIndex;
        }

        public MeshPart()
        {

        }

        public int LayerID { get { return layerID; } }

        public int MaterialIndex
        {
            get { return materialIndex; }
            set { materialIndex = value; }
        }      

        public int StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; }
        }

        public int PrimitiveCount
        {
            get { return primitiveCount; }
            set { primitiveCount = value; }
        }

        public int StartVertex
        {
            get { return startVertex; }
            set { startVertex = value; }
        }

        public int VertexCount
        {
            get { return vertexCount; }
            set { vertexCount = value; }
        }

        public int IndexCount { get { return indexCount; } set { indexCount = value; } }
    }
}
