using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Igneel.Graphics
{     
    public abstract class ShaderProgram
    {               
        protected InputLayout _inputLayout;
        protected Shader[] shaders;        

        protected ShaderProgram(ShaderProgramDesc desc)
        {
            _inputLayout = desc.Input;
            shaders = desc.Shaders.ToArray();            
        }

        public InputLayout InputDefinition
        {
            get
            {
                return _inputLayout;
            }           
        }

        public int ShadersCount
        {
            get
            {
                return shaders != null ? shaders.Length : 0;
            }
        }
       
        public Shader[] GetShaders()
        {
            return (Shader[])shaders.Clone();
        }

        public Shader GetShader(int idx)
        {
            return shaders[idx];
        }

        public abstract IUniformSetter CreateUniformSetter(string name);

        //public abstract IResourceSetter CreateResourceSetter(string name);

        protected abstract bool IsUniformDefined(string name);
       
        public abstract ShaderReflectionVariable[] GetUniformDescriptions();

        public abstract ShaderReflectionVariable GetUniformDescription(string name);

        //public abstract UniformDesc GetUniformDescription(int index);

        //public abstract int GetUniformCount();

        //public T Map<T>() where T:class
        //{
        //    T value = null;
        //    MappingInfo map;
        //    if (mappings.TryGetValue(typeof(T), out map))
        //    {
        //        value = map.Supported ? (T)map.Value : null;
        //    }
        //    else
        //    {
        //        value = CreateMapInstance<T>();
        //        if (value != null)                
        //            map = new MappingInfo { Value = value, Supported = true };                
        //        else
        //            map = new MappingInfo { Supported = false };

        //        mappings.Add(typeof(T), map);                
        //    }

        //    return value;
        //}

        //private T CreateMapInstance<T>() where T : class
        //{
        //    var properties = typeof(T).GetProperties();
        //    BinderInfo[] shaderInfos = shaders.Select(x => new BinderInfo { Shader = x }).ToArray();                       

        //    List<int>[] propShaders = new List<int>[properties.Length];

        //    if (!ValidateType(properties, shaderInfos, propShaders))
        //        return null;

        //    Type type = GenerateDynamicProxy<T>(properties, shaderInfos , propShaders);
        //    T instance = (T)Activator.CreateInstance(type);

        //    foreach (var info in shaderInfos)
        //    {
        //        if (info.Binder != null)
        //            type.GetField(info.BinderField.Name).SetValue(instance, info.Binder);
        //    }

        //    return instance;
        //}

        //private bool ValidateType(PropertyInfo[] properties, BinderInfo[] shaderInfos, List<int>[] propShaders)
        //{
        //    bool[] binded = new bool[properties.Length];
        //    List<string>[] varNames = new List<string>[shaderInfos.Length];

        //    for (int i = 0; i < properties.Length; i++)
        //    {
        //        var p = properties[i];
        //        for (int ishader = 0; ishader < shaderInfos.Length; ishader++)
        //        {
        //            if (IsShaderVariableDefined(p.Name, shaderInfos[ishader].Shader))
        //            {
        //                if (propShaders[i] == null)
        //                    propShaders[i] = new List<int>();
        //                if (varNames[ishader] == null)
        //                    varNames[ishader] = new List<string>();

        //                binded[i] = true;                        
        //                propShaders[i].Add(ishader);           
        //                varNames[ishader].Add(p.Name);
        //            }
        //        }
        //    }

        //    if (binded.All(x => x))
        //    {
        //        for (int i = 0; i < shaderInfos.Length; i++)
        //        {
        //            if(varNames[i]!=null)
        //                shaderInfos[i].Binder = CreateBinder(varNames[i].ToArray(), shaderInfos[i].Shader);
        //        }                
        //        return true;
        //    }

        //    return false;
        //}

        //private Type GenerateDynamicProxy<T>(PropertyInfo[] properties, BinderInfo[] shaderInfos, List<int>[] propShaders)
        //{
        //    var baseType = typeof(T);
        //    string typeName = "DynamicModule." + "<Dynamic>" + baseType.Name + DynamicMapper.id++;

        //    var mb = DynamicMapper.mb;

        //    TypeBuilder tb = baseType.IsInterface ?
        //        mb.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public, typeof(object), new Type[] { baseType }) :
        //        mb.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public, baseType);

        //    for (int i = 0; i < shaderInfos.Length; i++)
        //    {
        //        var info = shaderInfos[i];
        //        if (info.Binder != null)
        //        {
        //            var binderType = info.Binder.GetType();

        //            info.BinderField = tb.DefineField(DynamicMapper.BinderField + i, binderType, FieldAttributes.Static | FieldAttributes.Public | FieldAttributes.SpecialName);
        //            info.SetConstant = binderType.GetMethod("SetConstant", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        //            info.SetConstantI = binderType.GetMethod("SetConstantI", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        //            info.SetConstantIArray = binderType.GetMethod("SetConstantIArray", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        //            info.SetConstantB = binderType.GetMethod("SetConstantB", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        //            info.SetConstantBArray = binderType.GetMethod("SetConstantBArray", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        //        }
        //    }            

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

        //        // define getter
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

        //        LocalBuilder loc = null;
        //        Type elemType = null;
        //        if (prop.PropertyType.IsArray)
        //        {
        //            elemType = prop.PropertyType.GetElementType();
        //            var byRefElemType = elemType.MakeByRefType();
        //            loc = il.DeclareLocal(byRefElemType, true);
        //        }           

        //        for (int k = 0; k < propShaders[i].Count; k++)
        //        {
        //            var info = shaderInfos[k];
        //            if (prop.PropertyType.IsArray)
        //            {                                                         
        //                il.Emit(OpCodes.Ldarg_1); // load value                        
        //                il.Emit(OpCodes.Ldc_I4_0); //load index 0
        //                il.Emit(OpCodes.Ldelema, elemType); //load addr of value[0]
        //                il.Emit(OpCodes.Stloc, loc);

        //                il.Emit(OpCodes.Ldsfld, info.BinderField); //load binder ref
        //                il.Emit(OpCodes.Ldc_I4, i); //load index i

        //                il.Emit(OpCodes.Ldloc_0);//load addr of value[0]
        //                il.Emit(OpCodes.Conv_I); // convert to void* 

        //                if (elemType == typeof(int) || elemType == typeof(bool))
        //                {
        //                    il.Emit(OpCodes.Ldarg_1);
        //                    il.Emit(OpCodes.Ldlen); //load size (num elements)
        //                    method = elemType == typeof(int) ? info.SetConstantIArray : info.SetConstantBArray;
        //                }
        //                else
        //                {
        //                    il.Emit(OpCodes.Sizeof, elemType);
        //                    il.Emit(OpCodes.Ldarg_1);
        //                    il.Emit(OpCodes.Ldlen);

        //                    il.Emit(OpCodes.Mul);//load size (num bytes)

        //                    method = info.SetConstant;
        //                }

        //            }
        //            else
        //            {
        //                il.Emit(OpCodes.Ldsfld, info.BinderField); //load binder ref
        //                il.Emit(OpCodes.Ldc_I4, i); //load index

        //                if (prop.PropertyType == typeof(int))
        //                {
        //                    method = info.SetConstantI;
        //                    il.Emit(OpCodes.Ldarg_1);
        //                }
        //                else if (prop.PropertyType == typeof(bool))
        //                {
        //                    method = info.SetConstantB;
        //                    il.Emit(OpCodes.Ldarg_1);
        //                }
        //                else
        //                {
        //                    method = info.SetConstant;
        //                    il.Emit(OpCodes.Ldarga_S, 1);
        //                    il.Emit(OpCodes.Sizeof, prop.PropertyType);
        //                }
        //            }

        //            il.EmitCall(OpCodes.Call, method, null);   
        //        }                
        //        il.Emit(OpCodes.Ret);
        //    }

        //    Type type = tb.CreateType();
        //    return type;

        //}                              

        //protected EffectUniformSetter CreateVariableBinder(string name)
        //{
        //    List<IUniformSetter> binders = new List<IUniformSetter>();
        //    foreach (var item in shaders)
        //    {
        //        if (IsUniformDefined(name))
        //            binders.Add(CreateVariableBinder(name, item.Function));
        //    }

        //    return new EffectUniformSetter(binders.ToArray());
        //}        

        //private bool IsShaderVariableDefined(string name, List<Shader> shaders = null)
        //{
        //    bool defined = false;
        //    foreach (var shader in this.shaders)
        //    {
        //        if (IsShaderVariableDefined(name, shader.Function))
        //        {
        //            defined = true;
        //            if (shaders != null)
        //                shaders.Add(shader.Function);
        //        }
        //    }

        //    return defined;
        //}


        //private ProgramConstantDesc[] GetConstantDesc()
        //{
        //    ProgramConstantDesc[] d = new ProgramConstantDesc[shaders.Length];
        //    for (int i = 0; i < d.Length; i++)
        //    {
        //        d[i] = new ProgramConstantDesc
        //        {
        //            Shader = shaders[i],
        //            ConstantDescriptions = shaders[i].GetConstantDesc()
        //        };
        //    }
        //    return d;
        //}
    }

    //class BinderInfo
    //{
    //    public Shader Shader;
       
    //    public FieldBuilder BinderField;
    //    public IUniformsTable Binder;

    //    public MethodInfo SetConstant;

    //    public MethodInfo SetConstantI;
    //    public MethodInfo SetConstantIArray;

    //    public MethodInfo SetConstantB;
    //    public MethodInfo SetConstantBArray;
     
    //}    

    //public class ProgramUniformDesc
    //{
    //    public Shader Shader;
    //    public UniformDesc[] ConstantDescriptions;
    //}
    
}
