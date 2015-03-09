using Igneel.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Igneel.Graphics
{          
    public class VertexDescriptor
    {
        static class CACHE<T>
        {
            public static VertexDescriptor vd;
            public static int counter;
        }

        public enum Layout { Position = 0x1, Tangent = 0x2, Normal = 0x4, TexCoord = 0x8 }      
      
        int size;
        VertexElement[] elements;
        Layout fvf;
        bool[] presentSemantics = new bool[13];
        Type vertexType;             

        public VertexDescriptor(VertexElement[] elements)
        {           
            this.elements = elements;
            size = CalculateSize(elements);
        }

        public VertexDescriptor(Type type)
        {
            this.vertexType = type;
            var fields = vertexType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Array.Sort(fields, (f1, f2) => ((int)Marshal.OffsetOf(vertexType, f1.Name)).CompareTo((int)Marshal.OffsetOf(vertexType, f2.Name)));
            var offsets = fields.Select(f => (short)Marshal.OffsetOf(vertexType, f.Name)).ToArray();

            elements = new VertexElement[fields.Length];            

            for (int i = 0; i < fields.Length; i++)
            {
                VertexElementAttribute attr;
                try
                {
                    attr = (VertexElementAttribute)fields[i].GetCustomAttributes(typeof(VertexElementAttribute), false)[0];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException("The field" + fields[i].Name + " do not have a defined VertexElementAttribute");
                }

                elements[i] = new VertexElement(attr.Stream,
                                              attr.Offset < 0 ? offsets[i] : attr.Offset,
                                              attr.Format == IAFormat.Unused ? GetTypeFormat(fields[i].FieldType) : attr.Format,                                             
                                              attr.Semantic,
                                              attr.UsageIndex);
            }

            size = Marshal.SizeOf(vertexType);
        }

        int CalculateSize(VertexElement[] elements)
        {
            int size = 0;
            for (int i = 0; i < elements.Length - 1; i++)
            {
                VertexElement e = elements[i];
                presentSemantics[(int)e.Semantic] = true;
                if (e.Format != IAFormat.Unused)
                {
                    size += SizeOfElement(e.Format);

                    switch (e.Semantic)
                    {
                        case IASemantic.Normal:
                            fvf |= Layout.Normal;
                            break;
                        case IASemantic.Tangent:
                            fvf |= Layout.Tangent;
                            break;
                        case IASemantic.TextureCoordinate:
                            fvf |= Layout.TexCoord;
                            break;
                    }
                }
            }
            return size;
        }

        public Type VertexType
        {
            get { return vertexType; }
            set { vertexType = value; }
        }

        /// <summary>
        /// Size of the Vertex in Bytes
        /// </summary>
        public int Size { get { return size; } }

        public VertexElement[] Elements { get { return elements; } }     

        public int OffsetOf(IASemantic semantic, int usageIndex)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Semantic == semantic && elements[i].UsageIndex == usageIndex)
                {
                    return elements[i].Offset;
                }
            }

            return -1;
        }

        public int SizeOf(IASemantic semantic, int usageIndex)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].Semantic == semantic && elements[i].UsageIndex == usageIndex)
                {
                    return SizeOfElement(elements[i].Format);
                }
            }

            return -1;
        }

        public Layout Format { get { return fvf; } }

        public bool HaveNormal { get { return (fvf & Layout.Normal) == Layout.Normal; } }

        public bool HaveTangent { get { return (fvf & Layout.Tangent) == Layout.Tangent; } }

        public bool HaveTexCoord { get { return (fvf & Layout.TexCoord) == Layout.TexCoord; } }

        public bool HaveDeclarationUsage(IASemantic declarationUsage)
        {
            return presentSemantics[(int)declarationUsage];
        }

        public static int SizeOfElement(IAFormat type)
        {
            switch (type)
            {
                case IAFormat.Float3:
                    return 12;
                case IAFormat.Float4:
                    return 16;
                case IAFormat.HalfFour:
                case IAFormat.Short4N:
                case IAFormat.UShort4N:
                case IAFormat.Short4:
                case IAFormat.Float2:
                    return 8;
                case IAFormat.HalfTwo:
                case IAFormat.Color:
                case IAFormat.Ubyte4:
                case IAFormat.UByte4N:
                case IAFormat.Short2:
                case IAFormat.Short2N:
                case IAFormat.UShort2N:
                case IAFormat.Float1:
                    return 4;

            }
            throw new ArgumentOutOfRangeException("type");
        }

        public static IAFormat GetTypeFormat(Type type)
        {
            if (type == typeof(float))
                return IAFormat.Float1;
            if (type == typeof(Vector2))
                return IAFormat.Float2;
            if (type == typeof(Vector3))
                return IAFormat.Float3;
            if (type == typeof(Vector4) || type == typeof(Color4) || type == typeof(Int4))
                return IAFormat.Float4;
            if (type == typeof(int) || type == typeof(uint))
                return IAFormat.Color;
            if (type == typeof(Short4))
                return IAFormat.Short4;
            if (type == typeof(Byte4))
                return IAFormat.Ubyte4;

            throw new ArgumentOutOfRangeException("type");

        }             

        public static VertexDescriptor GetDescriptor<TVertex>() where TVertex : struct
        {
            VertexDescriptor vd = CACHE<TVertex>.vd;
            if (vd == null)
            {
                vd = new VertexDescriptor(typeof(TVertex));
                CACHE<TVertex>.vd = vd;

            }
            CACHE<TVertex>.counter++;
            return vd;
        }

        public static VertexDescriptor GetDescriptor(Type type)
        {
            var ctype = typeof(CACHE<>);
            var genType = ctype.MakeGenericType(type);

            VertexDescriptor vd = (VertexDescriptor)genType.GetField("vd").GetValue(null);
            if (vd == null)
            {
                vd = new VertexDescriptor(type);
                genType.GetField("vd").SetValue(null, vd);
            }
            var counter = (int)genType.GetField("counter").GetValue(null);
            genType.GetField("counter").SetValue(null, counter + 1);
            return vd;
        }

        public static void ReleaseDescriptor<TVertex>() where TVertex : struct
        {
            CACHE<TVertex>.counter--;
            if (CACHE<TVertex>.counter <= 0)
            {
                CACHE<TVertex>.counter = 0;
                CACHE<TVertex>.vd = null;
            }
        }
    }

    //[Serializable]
    //public class VertexDescriptorActivator : IProviderActivator
    //{
    //    [Serializable]
    //    [StructLayout(LayoutKind.Sequential)]
    //    struct VertexElementStore
    //    {        
    //        public short Offset { get; set; }
    //        public short Stream { get; set; }
    //        public IAFormat Type { get; set; }
    //        public IASemantic Usage { get; set; }
    //        public byte UsageIndex { get; set; }
    //    }

    //    VertexElementStore[] vertexElements;

    //    public void Initialize(IAssetProvider provider)
    //    {
    //        var vd = (VertexDescriptor)provider;
    //        vertexElements = new VertexElementStore[vd.Elements.Length];          
    //        for (int i = 0; i < vd.Elements.Length; i++)
    //        {
    //            vertexElements[i] = new VertexElementStore
    //            {
    //                Stream = vd.Elements[i].Stream,
    //                Offset = vd.Elements[i].Offset,
    //                Type = vd.Elements[i].Format,                  
    //                Usage = vd.Elements[i].Semantic,
    //                UsageIndex = vd.Elements[i].UsageIndex
    //            };
    //        }
    //    }

    //    public IAssetProvider CreateInstance()
    //    {
    //        VertexElement[] elements = new VertexElement[vertexElements.Length];
    //        for (int i = 0; i < vertexElements.Length; i++)
    //        {
    //            var e = vertexElements[i];
    //            elements[i] = new VertexElement(e.Stream, e.Offset, e.Type,  e.Usage, e.UsageIndex);
    //        }

    //        return new VertexDescriptor(elements);
    //    }
       
    //}    
    
}
