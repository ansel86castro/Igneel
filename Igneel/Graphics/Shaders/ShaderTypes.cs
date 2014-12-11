using Igneel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public class ShaderType
    {
        static List<ShaderType> shaderTypes = new List<ShaderType>();

        public Type NetType { get; set; }
        public ShaderMember[] Members { get; set; }
        public int Bytes { get; set; }

        private ShaderType() { }

        public ShaderType(Type netType)
        {
            if (!netType.IsValueType)
                throw new ArgumentException("The netType must be an struct");

            NetType = netType;
            Bytes = Marshal.SizeOf(netType);

            bool std  = false;
            for (int i = 0; i < 7; i++)
            {
                if (netType == shaderTypes[i].NetType)
                {
                    std = true;
                    break;
                }
            }

            if (!std)
            {
                var fields = netType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                Members = new ShaderMember[fields.Length];
                for (int i = 0; i < fields.Length; i++)
                {
                    Members[i] = new ShaderMember(fields[i]);
                }
            }
        }

        static ShaderType()
        {
            shaderTypes.Add(new ShaderType { NetType = typeof(int), Bytes = Marshal.SizeOf(typeof(int)) });
            shaderTypes.Add(new ShaderType { NetType = typeof(float), Bytes = Marshal.SizeOf(typeof(float))});
            shaderTypes.Add(new ShaderType { NetType = typeof(bool), Bytes = Marshal.SizeOf(typeof(bool)) });            
            shaderTypes.Add(new ShaderType { NetType = typeof(Vector2), Bytes = Marshal.SizeOf(typeof(Vector2))});
            shaderTypes.Add(new ShaderType { NetType = typeof(Vector3), Bytes = Marshal.SizeOf(typeof(Vector3))});
            shaderTypes.Add(new ShaderType { NetType = typeof(Vector4), Bytes = Marshal.SizeOf(typeof(Vector4))});
            shaderTypes.Add(new ShaderType { NetType = typeof(Matrix), Bytes = Marshal.SizeOf(typeof(Matrix))});

            RegisterType(typeof(LayerSurface));
            RegisterType(typeof(ShaderLight));
        }

        public static void RegisterType(Type type)
        {
            shaderTypes.Add(new ShaderType(type));
        }

        public static ShaderType GetShaderType(UniformDesc desc)
        {
            foreach (var t in shaderTypes)
            {
                if (t.Match(desc))
                    return t;
            }
            return null;
        }

        private bool Match(UniformDesc desc)
        {
            if (desc.Bytes != Bytes)
                return false;

            if ((Members == null && desc.Members != null) || 
                (Members!=null && desc.Members == null ) ||
                ((Members!=null && desc.Members!=null) && Members.Length != desc.Members.Length )) return false;

            if (Members != null)
            {
                for (int i = 0; i < Members.Length; i++)
                {
                    if (Members[i].Name != desc.Members[i].Name ||
                        !Members[i].Type.Match(desc.Members[i]))
                        return false;
                }
            }
            return true;
        }
    }

    public class ShaderMember
    {
        public ShaderType Type { get; set; }
        public string Name { get; set; }
        public int Bytes { get; set; }

        public ShaderMember(FieldInfo fi)
        {            
            Name = fi.Name;
            Bytes = Marshal.SizeOf(fi.FieldType);            
            Type = new ShaderType(fi.FieldType);
        }
        //public ShaderType ResolveType(Type t)
        //{
        //    shaderTypes.Add(new ShaderType { NetType = typeof(int), Bytes = Marshal.SizeOf(typeof(int)) });
        //    shaderTypes.Add(new ShaderType { NetType = typeof(float), Bytes = Marshal.SizeOf(typeof(float)) });
        //    shaderTypes.Add(new ShaderType { NetType = typeof(bool), Bytes = Marshal.SizeOf(typeof(bool)) });
        //    shaderTypes.Add(new ShaderType { NetType = typeof(Vector2), Bytes = Marshal.SizeOf(typeof(Vector2)) });
        //    shaderTypes.Add(new ShaderType { NetType = typeof(Vector3), Bytes = Marshal.SizeOf(typeof(Vector3)) });
        //    shaderTypes.Add(new ShaderType { NetType = typeof(Vector4), Bytes = Marshal.SizeOf(typeof(Vector4)) });
        //    shaderTypes.Add(new ShaderType { NetType = typeof(Matrix), Bytes = Marshal.SizeOf(typeof(Matrix)) });
        //}
        
    }

}
