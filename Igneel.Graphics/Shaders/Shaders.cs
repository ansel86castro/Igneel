using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Igneel.Graphics
{
    public interface Shader : IResourceAllocator
    {

    }

    public abstract class ShaderBase : ResourceAllocator ,Shader
    {
        //const string BinderField = "_<inject>ConstantBinder";
        //public List<IShaderVariableTable> binders = new List<IShaderVariableTable>();

        //sealed class MapCache<T>where T : class
        //{
        //    public static Dictionary<Shader, MapCache<T>> Mappings = new Dictionary<Shader,MapCache<T>>();
        //    public static Type GeneratedType;

        //    public T Value;
        //    public bool IsSupported = true;
        //    public IShaderVariableTable Binder;

        //    public static MapCache<T> GetMapping(Shader funtion)
        //    {
        //        MapCache<T> map = null;
        //        Mappings.TryGetValue(funtion, out map);
        //        return map;
        //    }

        //    public static void SetMapping(Shader function, MapCache<T> map)
        //    {
        //        Mappings[function] = map;
        //    }

        //    public void ClearMapping(Shader function)
        //    {
        //        Mappings.Remove(function);
        //    }

        //}        

        //static AssemblyName aName;
        //static ModuleBuilder mb;
        //static AssemblyBuilder assembly;                

        //public T Map<T>() where T : class
        //{
        //    var map = MapCache<T>.GetMapping(this);
        //    if (map != null)
        //    {
        //        return map.IsSupported ? map.Value : null;
        //    }
           
        //    if (ValidateMapping<T>())
        //    {
        //        var properties = typeof(T).GetProperties();
        //        var binder = CreateBinder();
        //        binders.Add(binder);
        //        InitializeBinding(binder, properties);

        //        T value;
        //        if (MapCache<T>.GeneratedType != null)                
        //        {
        //            //mapping is valid for this function                       
        //            value = (T)Activator.CreateInstance(MapCache<T>.GeneratedType);                    
        //        }
        //        else
        //        {
        //            value = GenerateDynamicProxy<T>(properties, binder);
        //            MapCache<T>.GeneratedType = value.GetType();
        //        }

        //        MapCache<T>.GeneratedType.GetField(BinderField).SetValue(null, binder);
        //        map = new MapCache<T> { Value = value, IsSupported = true };
        //    }
        //    else
        //    {
        //        //mapping is not valid for this function
        //        map = new MapCache<T> { Value = null, IsSupported = false };
        //    }

        //    MapCache<T>.SetMapping(this, map);
        //    return map.Value;
        //}

        //public IShaderVariableTable GetBinder<T>()where T:class
        //{
        //    MapCache<T> map = MapCache<T>.GetMapping(this);
        //    if (map != null)
        //        return map.Binder;
        //    return null;
        //}       

        //internal static T GenerateDynamicProxy<T>(PropertyInfo[] properties, IShaderVariableTable binder)
        //{            
        //    var baseType = typeof(T);                   
        //    string typeName = "DynamicModule." + "<Dynamic>" + baseType.Name;

        //    if (aName == null)
        //    {
        //        //aName = typeof(Generator).Assembly.GetName();
        //        aName = new AssemblyName("DynamicShaderSetter");
        //    }
        //    if (mb == null)
        //    {
        //        AppDomain myDomain = Thread.GetDomain();
        //        assembly = myDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);
        //        mb = assembly.DefineDynamicModule("DynamicModule", "DynamicModule.dll");
        //    }

        //    TypeBuilder tb = baseType.IsInterface ?
        //        mb.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public, typeof(object), new Type[] { baseType }) :
        //        mb.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public, baseType);

        //    var binderField = tb.DefineField(BinderField, binder.GetType(), FieldAttributes.Static | FieldAttributes.Public | FieldAttributes.SpecialName);
        //    var SetConstant = binder.GetType().GetMethod("SetConstant", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        //    var SetConstantI = binder.GetType().GetMethod("SetConstantI", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        //    var SetConstantIArray = binder.GetType().GetMethod("SetConstantIArray", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        //    var SetConstantB = binder.GetType().GetMethod("SetConstantB", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        //    var SetConstantBArray = binder.GetType().GetMethod("SetConstantBArray", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);          

        //    for (int i = 0; i < properties.Length; i++)
        //    {
        //        var prop = properties[i];             

        //        FieldInfo field;
        //        if (baseType.IsInterface)
        //        {
        //            //define fields
        //            field = tb.DefineField(prop.Name + "$p<" + i + ">", prop.PropertyType, FieldAttributes.Private | FieldAttributes.Static);
        //        }
        //        else
        //        {
        //            field = tb.GetField(String.Format("<{0}>k__BackingField", prop.Name));
        //        }
        //        //define getter
        //        var getter = tb.DefineMethod("get_" + prop.Name,
        //                     MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.ReuseSlot,
        //                     prop.PropertyType,
        //                     Type.EmptyTypes);

        //        if (!baseType.IsInterface)
        //            tb.DefineMethodOverride(getter, baseType.GetMethod(getter.Name));
                

        //        var il = getter.GetILGenerator();
        //        il.Emit(OpCodes.Ldsfld, field);
        //        il.Emit(OpCodes.Ret);

        //        //define setter
        //        var setter = tb.DefineMethod("set_" + prop.Name,
        //                    MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.ReuseSlot,
        //                    typeof(void),
        //                    new Type[] { prop.PropertyType });

        //        if (!baseType.IsInterface)
        //            tb.DefineMethodOverride(setter, baseType.GetMethod(setter.Name));

        //        il = setter.GetILGenerator();
        //        il.Emit(OpCodes.Ldarg_1);
        //        il.Emit(OpCodes.Stsfld, field);

        //        MethodInfo method;

        //        if (prop.PropertyType.IsArray)
        //        {
        //            var elemType = prop.PropertyType.GetElementType();
        //            var byRefElemType = elemType.MakeByRefType();
        //            var loc = il.DeclareLocal(byRefElemType, true);

        //            il.Emit(OpCodes.Ldarg_1); // load value                        
        //            il.Emit(OpCodes.Ldc_I4_0); //load index 0
        //            il.Emit(OpCodes.Ldelema, elemType); //load addr of value[0]
        //            il.Emit(OpCodes.Stloc, loc);

        //            il.Emit(OpCodes.Ldsfld, binderField); //load binder ref
        //            il.Emit(OpCodes.Ldc_I4, i); //load index i

        //            il.Emit(OpCodes.Ldloc_0);//load addr of value[0]
        //            il.Emit(OpCodes.Conv_I); // convert to void* 

        //            if (elemType == typeof(int) || elemType == typeof(bool))
        //            {
        //                il.Emit(OpCodes.Ldarg_1);
        //                il.Emit(OpCodes.Ldlen); //load size (num elements)
        //                method = elemType == typeof(int) ? SetConstantIArray : SetConstantBArray;
        //            }
        //            else
        //            {
        //                il.Emit(OpCodes.Sizeof, elemType);
        //                il.Emit(OpCodes.Ldarg_1);
        //                il.Emit(OpCodes.Ldlen);

        //                il.Emit(OpCodes.Mul);//load size (num bytes)

        //                method = SetConstant;
        //            }

        //        }
        //        else
        //        {
        //            il.Emit(OpCodes.Ldsfld, binderField); //load binder ref
        //            il.Emit(OpCodes.Ldc_I4, i); //load index

        //            if (prop.PropertyType == typeof(int))
        //            {
        //                method = SetConstantI;
        //                il.Emit(OpCodes.Ldarg_1);
        //            }
        //            else if (prop.PropertyType == typeof(bool))
        //            {
        //                method = SetConstantB;
        //                il.Emit(OpCodes.Ldarg_1);
        //            }
        //            else
        //            {
        //                method = SetConstant;
        //                il.Emit(OpCodes.Ldarga_S, 1);
        //                il.Emit(OpCodes.Sizeof, prop.PropertyType);
        //            }
        //        }

        //        il.EmitCall(OpCodes.Call, method, null);
        //        il.Emit(OpCodes.Ret);

        //    }            

        //    Type type = tb.CreateType();
        //    var instance = (T)Activator.CreateInstance(type);          
        //    return (T)instance;

        //}

        //private bool ValidateMapping<T>()
        //{
        //    try
        //    {
        //        CheckProperties(typeof(T).GetProperties());
        //        return true;
        //    }
        //    catch (InvalidMappingException)
        //    {
        //        return false;
        //    }
        //}

        //private void CheckProperties(PropertyInfo[] properties)
        //{
        //    for (int i = 0; i < properties.Length; i++)
        //    {
        //        var prop = properties[i];
        //        if (!prop.CanRead || !prop.CanWrite)
        //            throw new InvalidMappingException("property " + prop.Name + " must have getter and setter accesors");
        //        if (!prop.GetMethod.IsVirtual)
        //            throw new InvalidMappingException("property " + prop.Name + "getter must be virtual");
        //        if (!prop.SetMethod.IsVirtual)
        //            throw new InvalidMappingException("property " + prop.Name + "setter must be virtual");

        //        string name = prop.Name;
        //        var nameAttr = prop.GetCustomAttribute<VariableNameAttribute>();
        //        if (nameAttr != null)
        //            name = nameAttr.Name;

        //        if (IsConstantValid(name))
        //            throw new InvalidMappingException("The constant \""+ name + "\" is not defined in the shader as constant");

        //    }
        //}

        //internal static void InitializeBinding(IShaderVariableTable binder, PropertyInfo[] properties)
        //{
        //    binder.BeginDefinition(properties.Length);

        //    for (int i = 0; i < properties.Length; i++)
        //    {
        //        var prop = properties[i];
        //        string name = prop.Name;
        //        var nameAttr = prop.GetCustomAttribute<VariableNameAttribute>();
        //        if (nameAttr != null)
        //            name = nameAttr.Name;

        //        binder.DefineConstant(name, i);
        //    }

        //    binder.EndDefinitions();
        //}

        //protected abstract IShaderVariableTable CreateBinder(string [] names = null);

        //public abstract bool IsConstantValid(string name);      

        //public abstract int GetNbConstants();

        //public abstract ConstantDesc GetConstantDesc(int index);

        //public ConstantDesc[] GetConstantDesc()
        //{
        //    ConstantDesc[] c = new ConstantDesc[GetNbConstants()];           

        //    for (int i = 0; i < c.Length; i++)
        //    {
        //        c[i] = GetConstantDesc(i);              
        //    }                     

        //    return c;
        //}
    }

    /// <summary>
    /// Represent a compiled shader source
    /// </summary>
    /// 
    public interface VertexShader : Shader { }

    public interface PixelShader : Shader { }

    public interface GeometryShader : Shader { }

    public interface HullShader : Shader { }

    public interface DomainShader : Shader { }

    public interface ComputeShader : Shader { }
}
