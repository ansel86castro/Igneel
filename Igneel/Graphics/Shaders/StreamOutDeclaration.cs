using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public struct StreamOutDeclaration
    {
        public int Stream;
        public IASemantic Semantic;
        public int SemanticIndex;
        public byte StartComponent;
        public byte ComponentCount;
        public byte OutputSlot;        

        public static StreamOutDeclaration[] GetDeclaration(Type vertexType, out int[] bufferStrides )
        {            
            var fields = vertexType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Array.Sort(fields, (f1, f2) => ((int)Marshal.OffsetOf(vertexType, f1.Name)).CompareTo((int)Marshal.OffsetOf(vertexType, f2.Name)));
            var offsets = fields.Select(f => (short)Marshal.OffsetOf(vertexType, f.Name)).ToArray();

            List<StreamOutDeclaration> decs = new List<StreamOutDeclaration>(fields.Length);
            Dictionary<int, int> strides = new Dictionary<int, int>();

            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var fieldSize = Marshal.SizeOf(field.FieldType);
                var attr = field.GetCustomAttribute<OutputDeclarationAttribute>();
                if (attr == null) throw new InvalidOperationException();
                decs.Add(new StreamOutDeclaration
                {
                    Semantic = attr.Semantic,
                    SemanticIndex = attr.SemanticIndex,
                    ComponentCount = attr.ComponentCount > 0 ? attr.ComponentCount : (byte)(fieldSize / sizeof(float)),
                    Stream = attr.Stream,
                    OutputSlot = attr.OutputSlot,
                    StartComponent = attr.StartComponent
                });
                if (!strides.ContainsKey(attr.OutputSlot))
                    strides[attr.OutputSlot] = 0;

                strides[attr.OutputSlot] += fieldSize;
            }

            bufferStrides = strides.OrderBy(x => x.Key).Select(x => x.Value).ToArray();

            return decs.ToArray();
        }
    }

    public class OutputDeclarationAttribute : Attribute
    {
        public int Stream { get; set; }
        public IASemantic Semantic { get; set; }
        public int SemanticIndex { get; set; }
        public byte StartComponent { get; set; }
        public byte ComponentCount { get; set; }
        public byte OutputSlot { get; set; }
    }
}
