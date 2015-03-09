using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling.Declarations
{
    //public class VaryingType : PrimitiveTypeDeclaration
    //{
    //    public VaryingType(TypeClass[] classes, ShaderType[] types)
    //        : base("varying")
    //    {
    //        this.CompatibleTypes = types;
    //        this.CompatibleClasses = classes;
    //        ReflectionType = new ShaderReflectionType
    //        {
    //            Class = TypeClass.Undefined,
    //            Type = ShaderType.Unsupported,
    //            Name = Name,
    //            Members = new ShaderReflectionVariable[0]
    //        };
    //    }

    //    public TypeClass[] CompatibleClasses { get; set; }

    //    public ShaderType[] CompatibleTypes { get; set; }

    //    public override bool Match(TypeDeclaration other)
    //    {
    //        if (other.Name == Name)
    //        {
    //            VaryingType v = (VaryingType)other;
    //            if (CompatibleClasses.Length != v.CompatibleClasses.Length)
    //                return false;
    //            else if (CompatibleTypes.Length != v.CompatibleTypes.Length)
    //                return false;
    //            for (int i = 0; i < CompatibleClasses.Length; i++)
    //            {
    //                if (CompatibleClasses[i] != v.CompatibleClasses[i])
    //                    return false;
    //            }
    //            for (int i = 0; i < CompatibleTypes.Length; i++)
    //            {
    //                if (CompatibleTypes[i] != v.CompatibleTypes[i])
    //                    return false;
    //            }
    //            return true;
    //        }
    //        else
    //        {
    //            var index = Array.IndexOf(CompatibleClasses, other.Class);
    //            if (index < 0)
    //                return false;
    //            var typeClass = Array.IndexOf(CompatibleTypes, other.ReflectionType.Type);
    //            if (index < 0)
    //                return false;
    //            return true;
    //        }
    //    }

    //    public override TypeDeclaration GetCompatibleType(TypeDeclaration other)
    //    {
    //        if (Match(other))
    //            return other;

    //        return ShaderRuntime.Unknow;
    //    }
    //}
}
