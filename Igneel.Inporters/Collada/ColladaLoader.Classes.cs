using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Runtime.InteropServices;
using Igneel.Animations;
using Igneel.Assets;
using Igneel.Graphics;
using Igneel.Scenering;
using Igneel.Scenering.Animations;

namespace Igneel.Importers.Collada
{

    enum ColladaType
    {
        Unknown, Float, Name, Int, Bool, IDRef
    }

    class ParserOf : Attribute
    {
        public ParserOf(string tag)
        {
            this.Element = tag;
        }
        public string Element { get; set; }
    }

    class ElementRef
    {
        public XElement Element;

        public object Object;
    }

    class Source
    {
        public ColladaType Type;
        public string ColladaTypeString;
        public Accesor Accesor;
        public Array Array;
        public string Id;

        public IntPtr ArrayPointer;

        GCHandle handle;

        public bool IsLock()
        {
            return handle.IsAllocated;
        }

        public void Lock()
        {
            handle = GCHandle.Alloc(Array, GCHandleType.Pinned);
            ArrayPointer = Marshal.UnsafeAddrOfPinnedArrayElement(Array, 0);
        }

        public void Unlock()
        {
            handle.Free();
        }
    }

    class Accesor
    {
        public int Count;
        public int Stride;
        public Dictionary<string, AccesorParameter> Parameters = new Dictionary<string, AccesorParameter>();
    }

    class AccesorParameter
    {
        public string Name;
        public string Semantic;
        public ColladaType Type;
        public string ColladaTypeString;
    }

    class InputCollection
    {
        public SortedList<string, Source> Inputs = new SortedList<string, Source>();

        public Source this[string semantic]
        {
            get
            {
                Source source = null;
                Inputs.TryGetValue(semantic, out source);
                return source;
            }
        }

        public void Add(string semantic, Source source)
        {
            Inputs.Add(semantic, source);
        }

        public IEnumerable<KeyValuePair<string, Source>> GetInputs()
        {
            return Inputs;
        }

        public Input GetInput(string semantic, int offset = 0)
        {
            var source = this[semantic];
            return new Input
                {
                    Semantic = semantic,
                    Source = source,
                    Offset = offset,
                    UsageIndex = 0
                };
        }

        public IList<Source> Sources { get { return Inputs.Values; } }
    }

    struct Input
    {
        public string Semantic;
        public int Offset;
        public int UsageIndex;
        public Source Source;
        public InputCollection Definition;
    }

    class VertexBufferBuilder
    {
        public struct VertexCache
        {
            public int PosIndex;
            public int NormalIndex;
            public int TexCoordIndex;
            public int VertexBufferIndex;
        }

        bool[] taken;
        List<VertexCache>[] vertices;
        MemoryStream vertexBufferData;
        int count;
        private VertexDescriptor vd;

        public VertexBufferBuilder(int positionsCount, VertexDescriptor vd)
        {
            this.vd = vd;
            taken = new bool[positionsCount];
            vertices = new List<VertexCache>[positionsCount];
            vertexBufferData = new MemoryStream(positionsCount * vd.Size);
        }

        public int Count { get { return count; } }

        public int GetVertexIndex(int posIdx, int normalIdx, int texCoordIdx)
        {
            if (taken[posIdx])
            {
                var list = vertices[posIdx];
                foreach (var item in list)
                {
                    if (item.PosIndex == posIdx && item.NormalIndex == normalIdx && item.TexCoordIndex == texCoordIdx)
                    {
                        return item.VertexBufferIndex;
                    }
                }
            }
            return -1;
        }

        public int WriteVertex(byte[] vertexData, int posIdx, int normalIdx, int texCoordIdx)
        {
            if (vertices[posIdx] == null)
            {
                vertices[posIdx] = new List<VertexCache>();
                taken[posIdx] = true;
            }

            VertexCache vc = new VertexCache
            {
                PosIndex = posIdx,
                NormalIndex = normalIdx,
                TexCoordIndex = texCoordIdx,
                VertexBufferIndex = count
            };

            vertices[posIdx].Add(vc);
            vertexBufferData.Write(vertexData, 0, vertexData.Length);
            count++;
            return vc.VertexBufferIndex;
        }

        public byte[] GetBuffer()
        {
            return vertexBufferData.GetBuffer();
        }

        public List<VertexCache> GetVertices(int posIdx)
        {
            return vertices[posIdx];
        }
    }

    class CreateMeshResult
    {
        public Mesh Mesh;
        public VertexBufferBuilder VertexBuilder;
    }

    class CreateMeshInput
    {        
        public virtual Mesh CreateMesh()
        {
            return new Mesh(VertexDescriptor.GetDescriptor<MeshVertex>());
        }
    }

    class CreateSkelletalMeshInput : CreateMeshInput
    {
        public override Mesh CreateMesh()
        {
            return new Mesh(VertexDescriptor.GetDescriptor<SkinnedVertex>());
        }
    }

    class Channel
    {
        public InputCollection Sampler;
        public string Target;
    }

    class SamplerKeys
    {
        public int KeyInde;
        public AnimationCurve Sampler;
    }

    class AnimCurveResult
    {
        public string Name;        
        public List<float[]> Inputs = new List<float[]>();
        public List<SamplerKeys> Samplers = new List<SamplerKeys>();
        public List<ChannelInfo> Channels = new List<ChannelInfo>();
    }

    class ChannelInfo
    {
        public SamplerKeys Sampler;
        public string EntityName;
        public string TargetString;
    }

    public enum ParameterType 
    { 
        Float1, 
        Float2, 
        Float3, 
        Float4, 
        Surface, 
        Sampler2D,
        SamplerCUBE
    }

    class Sampler2D
    {
        public Uri Texture { get; set; }
    }

    class SamplerCube:Sampler2D
    {
        
    }

    class EffectParameter
    {                
        public string Id { get; set; }
        public List<ParameterAnnotation> Annotations { get; set; }
        public string Semantic { get; set; }
        public string Modifier { get; set; }
        public ParameterType Type { get; set; }
        public object Value { get; set; }
    }

    class ParameterAnnotation
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    static class XElementExtensor
    {
        public static string GetId(this XElement e)
        {
            var attr = e.Attribute("id");
            if (attr != null)
                return attr.Value;
            return null;
        }

        public static string GetName(this XElement e)
        {
            var attr = e.Attribute("name");
            if (attr != null)
                return attr.Value;
            return null;
        }

        public static XElement GetDescendantById(this XElement e, string id)
        {
            foreach (var item in e.Descendants())
            {
                if (item.GetId() == id)
                    return item;
            }
            return null;
        }

        public static XElement GetDescendantByAttr(this XElement e, string attr , string value)
        {
            foreach (var item in e.Descendants())
            {
                if (item.GetAttribute(attr) == value)
                    return item;
            }
            return null;
        }

        public static XElement GetDescendantByTag(this XElement e, string tag)
        {
            foreach (var item in e.Descendants())
            {
                if (item.Name.LocalName == tag)
                    return item;
            }
            return null;
        }

        public static bool FindElementByTag(this XElement e, string tags, Action<XElement> onFound)
        {
            string[] tagArray = tags.Split('/');
            XElement node = e;
            for (int i = 0; i < tagArray.Length; i++)
            {
                var desc = node.GetElementByTag(tagArray[i]);
                if (desc == null) return false;
                node = desc;
            }
            onFound(node);
            return true;
        }

        public static bool FindDescendantByTag(this XElement e, string tag, Action<XElement> onFound)
        {
            var node = e.GetDescendantByTag(tag);

            if (node == null)
                return false;

            onFound(node);
            return true;
        }

        public static IEnumerable<XElement> GetDescendantsByTag(this XElement e, string tag)
        {
            foreach (var item in e.Descendants())
            {
                if (item.Name.LocalName == tag)
                    yield return item;
            }
        }

        public static XElement GetElementByTag(this XElement e, string tag)
        {
            foreach (var item in e.Elements())
            {
                if (item.Name.LocalName == tag)
                    return item;
            }
            return null;
        }

        public static IEnumerable<XElement> GetElementsByTag(this XElement e, string tag)
        {
            return from n in e.Elements()
                   where n.Name.LocalName == tag
                   select n;
        }

        public static XElement GetElementById(this XElement e, string id)
        {
            foreach (var item in e.Elements())
            {
                if (item.GetId() == id)
                    return item;
            }
            return null;
        }

        public static string GetAttribute(this XElement e, string name)
        {
            var attr = e.Attribute(name);
            return attr != null ? attr.Value : null;
        }        

    }

    class ColladaExecutionContext:IAssetProvider
    {
        public bool z_up;           

        public unsafe Dictionary<string, ICurveOutput> GetChannelsCallbacks()
        {
            var dic = new Dictionary<string, Action<SceneNodeTransforms, IntPtr>>
            {
                {"translate.X", (x , v)=> x.TraslationX = *(float*)v },
                {"translate.Y", (x , v)=>{ if(z_up) x.TraslationZ = *(float*)v; else x.TraslationY = *(float*)v; }},
                {"translate.Z", (x , v)=>{ 
                    if(z_up) x.TraslationY = *(float*)v; 
                    else x.TraslationZ = *(float*)v; }},

                {"scale.X", (x , v)=> x.ScalingX = *(float*)v  },
                {"scale.Y", (x , v)=>{ if(z_up) x.ScalingZ = *(float*)v; else x.ScalingY = *(float*)v; } },
                {"scale.Z", (x , v)=>{ if(z_up) x.ScalingY = *(float*)v; else x.ScalingY = *(float*)v; } },
                
                {"rotateX.ANGLE", (x , v)=>{
                     x.ApplyRotation(Quaternion.RotationAxis(Vector3.UnitX, -Numerics.ToRadians(*(float*)v)));
                }},
                {"rotateY.ANGLE", (x , v)=> {
                    if (z_up)
                        x.ApplyRotation(Quaternion.RotationAxis(Vector3.UnitZ, -Numerics.ToRadians(*(float*)v)));                         
                    else
                        x.ApplyRotation(Quaternion.RotationAxis(Vector3.UnitY, -Numerics.ToRadians(*(float*)v)));                        
                }},
                {"rotateZ.ANGLE", (x , v) => {
                    if (z_up)
                        x.ApplyRotation(Quaternion.RotationAxis(Vector3.UnitY, -Numerics.ToRadians(*(float*)v)));                        
                    else
                        x.ApplyRotation(Quaternion.RotationAxis(Vector3.UnitZ, -Numerics.ToRadians(*(float*)v)));                                                          
                }},
                {"matrix", (x, v) => x.Pose = *(Matrix*)v  },
                {"rotate", (x, v) => x.ApplyRotation(*(Quaternion*)v) },
                {"translate",(x ,v)=> x.Traslation = *(Vector3*)v },
                {"scale", (x , v)=>  x.Scale = *(Vector3*)v },
            };

            var result = new Dictionary<string, ICurveOutput>();
            foreach (var item in dic)
            {
                result.Add(item.Key, new CurveOutput<SceneNodeTransforms>(item.Value));
            }
            return result;
        }

        public Asset CreateAsset()
        {
            return Asset.Create(this);
        }

        [Serializable]
        class Activator : IProviderActivator
        {
            bool zup;
            public void Initialize(IAssetProvider provider)
            {
                zup = ((ColladaExecutionContext)provider).z_up;
            }

            public IAssetProvider CreateInstance()
            {
                return new ColladaExecutionContext() { z_up = zup };
            }
        }
    }
}
