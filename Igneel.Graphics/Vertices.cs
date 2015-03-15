using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Reflection;

namespace Igneel.Graphics
{       
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct MeshVertex
    {
        [VertexElement(IASemantic.Position)]
        public Vector3 Position;

        [VertexElement(IASemantic.Normal)]
        public Vector3 Normal;

        [VertexElement(IASemantic.Tangent)]
        public Vector3 Tangent;

        [VertexElement(IASemantic.TextureCoordinate, 0)]
        public Vector2 TexCoord;

         [VertexElement(IASemantic.TextureCoordinate, 1)]
        public float OccFactor;

        public MeshVertex(float x, float y, float z,
                           float tx, float ty, float tz,
                           float nx, float ny, float nz,
                            float u, float v, float occ)
        {
            Position = new Vector3(x, y, z);
            Tangent = new Vector3(tx, ty, tz);
            Normal = new Vector3(nx, ny, nz);
            TexCoord = new Vector2(u, v);
            this.OccFactor = occ;
        }

        public MeshVertex(Vector3 position = default(Vector3), Vector3 normal = default(Vector3), Vector3 tangent = default(Vector3), Vector2 texCoord = default(Vector2), float occ = 0)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            TexCoord = texCoord;
            OccFactor = occ;
            
        }           

        public override string ToString()
        {
            return Position.ToString();
            //return "P:" + Position.ToString() + " N:" + Normal + " T:" + Tangent + " Tx:" + TexCoord;
        }     
    }   
 
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SkinnedVertex
    {
        [VertexElement(IASemantic.Position)]
        public Vector3 Position;

        [VertexElement(IASemantic.BlendWeight)]
        public Vector4 BlendWeights;

        [VertexElement(IASemantic.BlendIndices, Format = IAFormat.Float4)]
        public Vector4 BlendIndices;

        [VertexElement(IASemantic.Normal)]
        public Vector3 Normal;

        [VertexElement(IASemantic.Tangent)]
        public Vector3 Tangent;

        [VertexElement(IASemantic.TextureCoordinate, 0)]
        public Vector2 TexCoord;

        [VertexElement(IASemantic.TextureCoordinate, 1)]
        public float OccFactor;

        public SkinnedVertex(float x, float y, float z,
                           float tx, float ty, float tz,
                           float nx, float ny, float nz,
                            float u, float v, float occ)
        {
            Position = new Vector3(x, y, z);
            Tangent = new Vector3(tx, ty, tz);
            Normal = new Vector3(nx, ny, nz);
            TexCoord = new Vector2(u, v);
            this.OccFactor = occ;
            BlendIndices = new Vector4();
            BlendWeights = new Vector4();
        }

        public SkinnedVertex(Vector3 position = default(Vector3), Vector3 normal = default(Vector3), Vector3 tangent = default(Vector3), Vector2 texCoord = default(Vector2), float occ = 0)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            TexCoord = texCoord;
            OccFactor = occ;
            BlendIndices = new Vector4();
            BlendWeights = new Vector4();
        }      

        public override string ToString()
        {
            return Position.ToString();
            //return "P:" + Position.ToString() + " N:" + Normal + " T:" + Tangent + " Tx:" + TexCoord;
        }
    }  

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct VertexP
    {
        [VertexElement(IASemantic.Position)]
        public Vector3 Position;

        public VertexP(Vector3 position)
        {
            Position = position;
        }
      
        public override string ToString()
        {
            return Position.ToString();
        }
     
    }
    
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct VertexPNTTx 
    {
        [VertexElement(IASemantic.Position)]
        public Vector3 Position;
        [VertexElement(IASemantic.Normal)]
        public Vector3 Normal;
        [VertexElement(IASemantic.Tangent)]
        public Vector3 Tangent;
        [VertexElement(IASemantic.TextureCoordinate)]
        public Vector2 TexCoord;

        public VertexPNTTx(float x, float y, float z,
                           float tx, float ty, float tz,
                           float nx, float ny, float nz,
                            float u, float v)
        {
            Position = new Vector3(x, y, z);
            Tangent = new Vector3(tx, ty, tz);
            Normal = new Vector3(nx, ny, nz);
            TexCoord = new Vector2(u, v);
        }

        public VertexPNTTx(Vector3 position = default(Vector3), Vector3 normal = default(Vector3), Vector3 tangent = default(Vector3), Vector2 texCoord = default(Vector2))
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            TexCoord = texCoord;
        }            

        public override string ToString()
        {
            return Position.ToString();
            //return "P:" + Position.ToString() + " N:" + Normal + " T:" + Tangent + " Tx:" + TexCoord;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct VertexPNTx 
    {
        [VertexElement(IASemantic.Position)]
        public Vector3 Position;

        [VertexElement(IASemantic.Normal)]
        public Vector3 Normal;

        [VertexElement(IASemantic.TextureCoordinate)]
        public Vector2 TexCoord;

        public VertexPNTx(Vector3 position= default(Vector3), Vector3 normal= default(Vector3), Vector2 texCoord= default(Vector2))
        {
            Position = position;
            Normal = normal;            
            TexCoord = texCoord;
        }      

        public override string ToString()
        {
            return "P(" + Position.ToString() + ") N(" + Normal.ToString() + ") T(" + TexCoord.ToString()+")";
        }
    }

    [StructLayout(LayoutKind.Sequential),Serializable]
    public struct VertexPTx
    {
        [VertexElement(IASemantic.Position)]
        public Vector3 Position;

        [VertexElement(IASemantic.TextureCoordinate)]
        public Vector2 TexCoord;

        public VertexPTx(Vector3 position = default(Vector3), Vector2 texCoord = default(Vector2))
        {
            Position = position;
            TexCoord = texCoord;
        }

        public VertexPTx(float x,float y,float z ,float u ,float v)
        {
            Position = new Vector3(x, y, z);
            TexCoord = new Vector2(u, v);
        }    

        public override string ToString()
        {
            return "P" + Position + " Tx" + TexCoord;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct VertexPTxH
    {
        [VertexElement(IASemantic.PositionTransformed)]
        public Vector4 Position;

        [VertexElement(IASemantic.TextureCoordinate)]
        public Vector2 TexCoord;

        public VertexPTxH(Vector4 position = default(Vector4), Vector2 texCoord = default(Vector2))
        {
            Position = position;
            TexCoord = texCoord;
        }           

        public override string ToString()
        {
            return "P" + Position + " Tx" + TexCoord;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct VertexPositionColor
    {
        [VertexElement(IASemantic.Position)]
        public Vector3 Position;

        [VertexElement(IASemantic.Color)]
        public Color4 Color;

        public VertexPositionColor(Vector3 pos, Vector4 color)
        {
            this.Position = pos;
            this.Color = (Color4)color;
        }

        public VertexPositionColor(Vector3 Position, Color4 color)
        {
            this.Position = Position;
            this.Color = color;
        }
        

        public override string ToString()
        {
            return "P" + Position + " Color" + Color;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct TriVertexP 
    {
        [VertexElement(IASemantic.Position,0)]
        public Vector3 P1;
        [VertexElement(IASemantic.Position, 1)]
        public Vector3 P2;
        [VertexElement(IASemantic.Position, 2)]
        public Vector3 P3;
        [VertexElement(IASemantic.Normal)]
        public Vector3 Normal;

        [VertexElement(IASemantic.TextureCoordinate, 0)]
        public float TriangleIndex;
      
        public void ComputeNormal()
        {
            Normal = Triangle.ComputeFaceNormal(P1, P2, P3);
        }

        public void Transform(Matrix matrix)
        {
            P1=Vector3.Transform(P1,matrix);
            P2 = Vector3.Transform(P2, matrix);
            P3 = Vector3.Transform(P3, matrix);
            Normal = Vector3.TransformNormal(Normal,matrix);
        }
    }
     
    [StructLayout(LayoutKind.Sequential)]
     [Serializable]
    public struct TriVertexNTTx
    {
        [VertexElement(IASemantic.Normal, 0)]
        public Vector3 Normal1;
         [VertexElement(IASemantic.Normal, 1)]
        public Vector3 Normal2;
         [VertexElement(IASemantic.Normal, 2)]
        public Vector3 Normal3;
         [VertexElement(IASemantic.Tangent, 0)]
        public Vector3 Tangent1;
         [VertexElement(IASemantic.Tangent, 1)]
        public Vector3 Tangent2;
         [VertexElement(IASemantic.Tangent, 2)]
        public Vector3 Tangent3;
         [VertexElement(IASemantic.TextureCoordinate, 0)]
        public Vector2 TexCood1;
         [VertexElement(IASemantic.TextureCoordinate, 1)]
        public Vector2 TexCood2;
         [VertexElement(IASemantic.TextureCoordinate, 2)]
        public Vector2 TexCood3;      
    }
     
    [StructLayout(LayoutKind.Sequential)]
     [Serializable]   
    public struct TriVertexNTTxColor
     {
         [VertexElement(IASemantic.Normal, 0)]
         public Vector3 Normal1;
         [VertexElement(IASemantic.Normal, 1)]
         public Vector3 Normal2;
         [VertexElement(IASemantic.Normal, 2)]
         public Vector3 Normal3;
         [VertexElement(IASemantic.Tangent, 0)]
         public Vector3 Tangent1;
         [VertexElement(IASemantic.Tangent, 1)]
         public Vector3 Tangent2;
         [VertexElement(IASemantic.Tangent, 2)]
         public Vector3 Tangent3;
         [VertexElement(IASemantic.TextureCoordinate, 0)]
         public Vector2 TexCood1;
         [VertexElement(IASemantic.TextureCoordinate, 1)]
         public Vector2 TexCood2;
         [VertexElement(IASemantic.TextureCoordinate, 2)]
         public Vector2 TexCood3;
         [VertexElement(IASemantic.Color)]
         public Vector4 Color;      

     }

     [StructLayout(LayoutKind.Sequential)]
     [Serializable]
     public struct TerrainVertex
     {
         [VertexElement(IASemantic.Position)]
         public Vector3 Position;
         [VertexElement(IASemantic.Normal)]
         public Vector3 Normal;
         [VertexElement(IASemantic.TextureCoordinate,0)]
         public Vector2 TexCoord;
         [VertexElement(IASemantic.TextureCoordinate,1)]
         public Vector2 BlendTexCoord;
         [VertexElement(IASemantic.TextureCoordinate, 2)]
         public float OccFactor;

         public TerrainVertex(Vector3 position = default(Vector3), Vector3 normal = default(Vector3), Vector2 texCoord = default(Vector2), Vector2 blendTexCoord = default(Vector2), float occ = 0)
         {
             Position = position;
             Normal = normal;
             TexCoord = texCoord;
             BlendTexCoord = blendTexCoord;
             OccFactor = occ;
         }       

         public override string ToString()
         {
             return "P(" + Position.ToString() + ") N(" + Normal.ToString() + ") T(" + TexCoord.ToString() + ")";
         }
     }    


     [StructLayout(LayoutKind.Sequential), Serializable]
     public struct PointSprite
     {
         [VertexElement(IASemantic.Position)]
         public Vector3 Position;
         [VertexElement(IASemantic.TextureCoordinate,0)]
         public Vector2 TexCoord;
         [VertexElement(IASemantic.Color)]
         public Color4 Color;
         [VertexElement(IASemantic.PointSize)]
         public float Size;
         [VertexElement(IASemantic.TextureCoordinate,1)]
         public float Rotation;
      
         public override string ToString()
         {
             return Position.ToString();
         }
     }
}
