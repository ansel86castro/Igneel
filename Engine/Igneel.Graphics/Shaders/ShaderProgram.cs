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
        class MappingInfo { public Object Value; public bool Supported; }

        protected InputLayout _inputLayout;
        protected Shader[] shaders;
        dynamic _dynamicMap;
        Dictionary<Type, MappingInfo> _mappinGS;

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

        public dynamic Input
        {
            get
            {
                if (_dynamicMap == null)
                {
                    Mapper mapper = new Mapper { program = this };
                    _dynamicMap = mapper.CreateMapType();
                }
                return _dynamicMap;
            }
        }

        public T Map<T>(bool fullmap = false) where T : class
        {
            T value = null;
            MappingInfo map;
            if (_mappinGS !=null && _mappinGS.TryGetValue(typeof(T), out map))
            {
                value = map.Supported ? (T)map.Value : null;
            }
            else
            {
                try
                {
                    Mapper mapper = new Mapper { program =this };
                    value = (T)mapper.CreateMapType(typeof(T), fullmap);
                    if (value != null)
                        map = new MappingInfo { Value = value, Supported = true };
                    else
                        map = new MappingInfo { Supported = false };
                }
                catch (InvalidMappingException)
                {
                    map = new MappingInfo { Supported = false };
                }

                if (_mappinGS == null)
                    _mappinGS = new Dictionary<Type, MappingInfo>(1);
                _mappinGS.Add(typeof(T), map);
            }

            return value;
        }

        #region Mapping

        class Mapper
        {
            public ShaderProgram program;

            List<VarInfo> _variables = new List<VarInfo>();
            Dictionary<string, VarInfo> _varlookup = new Dictionary<string, VarInfo>();
            internal const string BinderField = "_<cb>";
            internal static AssemblyName AName;
            internal static ModuleBuilder Mb;
            internal static AssemblyBuilder Assembly;
            static string _namespace;
            internal static int Id;
            static FieldInfo _binderField;
            static MethodInfo _setValueMethod;

            static Mapper()
            {
                Initialize();
            }

            public static void Initialize()
            {
                if (AName == null)
                {
                    AName = typeof(ShaderVariable).Assembly.GetName(true);
                    _namespace = typeof(ShaderVariable).Namespace;
                    // aName = new AssemblyName("Igneel.Graphics");
                }
                if (Mb == null)
                {
                    AppDomain myDomain = Thread.GetDomain();
                    Assembly = myDomain.DefineDynamicAssembly(AName, AssemblyBuilderAccess.Run);
                    Mb = Assembly.DefineDynamicModule("Igneel.Rendering");
                    //mb = assembly.DefineDynamicModule("Igneel.Rendering", "Igneel.Rendering.dll");
                }

                _binderField = typeof(ShaderVariable).GetField("Binder", BindingFlags.NonPublic | BindingFlags.Instance);
                _setValueMethod = typeof(ShaderVariable).GetMethod("SetValue");
            }

            #region CODE GEN

            private object GenType(Type baseType)
            {
                TypeBuilder tb;
                if (baseType != null)
                {
                    tb = baseType.IsInterface ?
                    Mb.DefineType(_namespace + "." + "_$" + baseType.Name + Id, TypeAttributes.Class | TypeAttributes.Public, typeof(object), new Type[] { baseType }) :
                    Mb.DefineType(_namespace + "." + "_$" + baseType.Name + Id, TypeAttributes.Class | TypeAttributes.Public, baseType);
                }
                else
                    tb = Mb.DefineType(_namespace + "." + "$dI" + Id, TypeAttributes.Class | TypeAttributes.Public);

                Id++;

                var commit = typeof(ShaderVariable).GetMethod("Commit");
                List<KeyValuePair<FieldBuilder, ShaderVariable>> createFields = new List<KeyValuePair<FieldBuilder, ShaderVariable>>();

                foreach (var v in _varlookup)
                {
                    if (v.Value.NetType != null)
                    {
                        ShaderVariable effectVar = v.Value.Variable;
                        var tField = tb.DefineField(v.Key + "bakingField", effectVar.GetType(), FieldAttributes.Static | FieldAttributes.Public | FieldAttributes.SpecialName);

                        createFields.Add(new KeyValuePair<FieldBuilder, ShaderVariable>(tField, effectVar));

                        PropertyBuilder prop = null;
                        PropertyInfo pi = null;
                        if (baseType == null)
                        {
                            prop = tb.DefineProperty(v.Key, PropertyAttributes.None, v.Value.NetType, null);
                            pi = prop;
                        }
                        else
                        {
                            pi = v.Value.NetProp;
                        }

                        //define getter
                        DefineGetter(tb, tField, prop, baseType, pi);

                        //define setter                               
                        DefineSetter(tb, commit, tField, prop, baseType, pi);
                    }
                    else if (baseType != null)
                    {
                        var prop = v.Value.NetProp ?? baseType.GetProperty(v.Key);
                        var tField = tb.DefineField(v.Key + "bakingField", prop.PropertyType, FieldAttributes.Static | FieldAttributes.Public | FieldAttributes.SpecialName);

                        #region Getter

                        MethodBuilder getter;
                        var attr = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.ReuseSlot;

                        getter = tb.DefineMethod(prop.GetMethod.Name, attr, prop.PropertyType, Type.EmptyTypes);

                        // define getter
                        // return field.Value;
                        var il = getter.GetILGenerator();
                        il.Emit(OpCodes.Ldsfld, tField);
                        il.Emit(OpCodes.Ret);

                        if (!baseType.IsInterface)
                            tb.DefineMethodOverride(getter, baseType.GetMethod(getter.Name));

                        #endregion

                        #region Setter

                        MethodBuilder setter = tb.DefineMethod(prop.SetMethod.Name, attr, typeof(void), new Type[] { prop.PropertyType });

                        // field = value                
                        il = setter.GetILGenerator();
                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Stsfld, tField);
                        il.Emit(OpCodes.Ret);

                        if (!baseType.IsInterface)
                            tb.DefineMethodOverride(setter, baseType.GetMethod(setter.Name));

                        #endregion
                    }
                }

                Type type = tb.CreateType();
                var inst = Activator.CreateInstance(type);
                foreach (var f in createFields)
                {
                    type.GetField(f.Key.Name).SetValue(null, f.Value);
                    //f.Key.SetValue(inst, f.Value);
                }

                return inst;
            }

            private static void DefineSetter(TypeBuilder tb, MethodInfo commit, FieldBuilder tField, PropertyBuilder prop, Type baseType, PropertyInfo pi)
            {
                var attr = prop == null ? MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.ReuseSlot :
                                        MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName;

                MethodBuilder setter = tb.DefineMethod("set_" + pi.Name, attr, typeof(void), new Type[] { pi.PropertyType });

                // field.Value = value                
                var il = setter.GetILGenerator();
                Label gotoRet = il.DefineLabel();

                il.Emit(OpCodes.Ldsfld, tField);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, tField.FieldType.GetField("Value"));

                // field.Commit();
                il.Emit(OpCodes.Ldsfld, tField);
                //il.EmitCall(OpCodes.Call, commit, null);
                il.Emit(OpCodes.Ldfld, tField.FieldType.GetField("Binder"));
                il.Emit(OpCodes.Brfalse, gotoRet);

                il.Emit(OpCodes.Ldsfld, tField);
                il.EmitCall(OpCodes.Call, tField.FieldType.GetMethod("SetValue"), null);

                il.MarkLabel(gotoRet);
                il.Emit(OpCodes.Ret);


                if (prop != null)
                {
                    prop.SetSetMethod(setter);
                }

                if (baseType != null && !baseType.IsInterface)
                    tb.DefineMethodOverride(setter, baseType.GetMethod(setter.Name));
            }

            private static void DefineGetter(TypeBuilder tb, FieldBuilder tField, PropertyBuilder prop, Type baseType, PropertyInfo pi)
            {
                MethodBuilder getter;
                var attr = prop == null ? MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.ReuseSlot :
                                        MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName;

                getter = tb.DefineMethod("get_" + pi.Name, attr, pi.PropertyType, Type.EmptyTypes);

                // define getter
                // return field.Value;
                var il = getter.GetILGenerator();
                il.Emit(OpCodes.Ldsfld, tField);
                il.Emit(OpCodes.Ldfld, tField.FieldType.GetField("Value"));
                il.Emit(OpCodes.Ret);

                if (prop != null)
                {
                    prop.SetGetMethod(getter);
                }

                if (baseType != null && !baseType.IsInterface)
                    tb.DefineMethodOverride(getter, baseType.GetMethod(getter.Name));
            }

            #endregion

            #region STATIC

            PropertyInfo[] GetProperties(Type type)
            {
                List<PropertyInfo> properties = new List<PropertyInfo>();
                var interfaces = type.GetInterfaces();
                if (interfaces.Length > 0)
                {
                    foreach (var item in interfaces)
                    {
                        var proPS = GetProperties(item);
                        properties.AddRange(proPS);
                    }
                }

                properties.AddRange(type.GetProperties());
                return properties.ToArray();
            }

            private void GetVariablesInformation(PropertyInfo[] proPS)
            {
                foreach (var p in proPS)
                {                   
                        VarInfo v;
                        ShaderReflectionVariable ud =program.GetUniformDescription(p.Name);

                        if (!_varlookup.TryGetValue(p.Name, out v))
                        {
                            v = new VarInfo()
                            {
                                Name = p.Name,
                                NetProp = p,
                                Desc = ud,
                                NetType = ud != null ? p.PropertyType : null,                               

                            };
                            if (v.Desc != null && v.NetType != null)
                            {
                                v.Variable = CreateVariable(v.NetType, v.Desc);
                                v.Variable.Name = ud.Name;
                                v.Variable.Binder = program.CreateUniformSetter(ud.Name);                                
                            }

                            _varlookup.Add(v.Name, v);
                        }                            
                    }                   
            }
          
            private ShaderVariable CreateVariable(Type type, ShaderReflectionVariable desc)
            {
                var shaderType = desc.Type;
                if (type.IsArray)
                {
                    if (shaderType.Elements == 0)
                        throw new InvalidOperationException("The constant " + desc.Name + "is not an array");
                    var e = type.GetElementType();
                    if (e == typeof(int))
                        return new IntArrayVariable() { Elements = shaderType.Elements };
                    else if (e == typeof(bool))
                        return new BoolArrayVariable() { Elements = shaderType.Elements };
                    else if (e == typeof(float))
                        return new FloatArrayVariable() { Elements = shaderType.Elements };
                    else if (e == typeof(Matrix))
                        return new MatrixArrayVariable() { Elements = shaderType.Elements };
                    else if (e == typeof(Vector4))
                        return new Vector4ArrayVariable() { Elements = shaderType.Elements };
                    else if (e == typeof(SamplerState))
                        return new SamplerStateVariableArray() { Elements = shaderType.Elements };
                    else if (e.GetInterface(typeof(IShaderResource).Name) != null)
                        return new ResourceVariableArray { Elements = shaderType.Elements };
                    else if (e.IsValueType)
                    {
                        var insType = typeof(ShaderVariableArray<>).MakeGenericType(e);
                        var inst = (ShaderVariableArray)Activator.CreateInstance(insType);
                        inst.Elements = shaderType.Elements;
                        return inst;
                    }
                    else
                        throw new InvalidOperationException("The type \"" + e.Name + "\" is not supported");
                }
                else
                {
                    if (type == typeof(int))
                        return new IntVariable();
                    else if (type == typeof(bool))
                        return new BoolVariable();
                    else if (type == typeof(float))
                        return new FloatVariable();
                    else if (type == typeof(Matrix))
                        return new MatrixVariable();
                    else if (type == typeof(Vector4))
                        return new Vector4Variable();
                    else if (type.IsGenericType)
                    {
                        var genDef = type.GetGenericTypeDefinition();
                        if (genDef == typeof(SArray<>))
                        {
                            var arGS = type.GetGenericArguments();
                            var insType = typeof(RangeVariable<>).MakeGenericType(arGS);
                            return (ShaderVariable)Activator.CreateInstance(insType);
                        }
                        else if (genDef == typeof(Sampler<>))
                        {
                            var arGS = type.GetGenericArguments();
                            var insType = typeof(SamplerVariable<>).MakeGenericType(arGS);
                            return (ShaderVariable)Activator.CreateInstance(insType);
                        }
                        else if (genDef == typeof(SamplerArray<>))
                        {
                            var arGS = type.GetGenericArguments();
                            var insType = typeof(SamplerVariableArray<>).MakeGenericType(arGS);
                            return (ShaderVariable)Activator.CreateInstance(insType);
                        }
                        else
                            throw new InvalidOperationException("The type \"" + type.Name + "\" is not supported");
                    }
                    else if (type == typeof(SamplerState))
                        return new SamplerStateVariable();
                    else if (type.GetInterface(typeof(IShaderResource).Name) != null)
                        return new ResourceVariable();
                    else if (type.IsValueType)
                    {
                        var insType = typeof(ShaderVariable<>).MakeGenericType(type);
                        return (ShaderVariable)Activator.CreateInstance(insType);
                    }
                    else
                        throw new InvalidOperationException("The type \"" + type.Name + "\" is not supported");
                }
            }


            public object CreateMapType(Type type, bool fullMapping = false)
            {
                var proPS = GetProperties(type);

                GetVariablesInformation(proPS);

                if (_varlookup.Values.All(x => x.Desc == null))
                    return null;

                if (fullMapping && _varlookup.Count < proPS.Length)
                    throw new InvalidMappingException("Invalid Mapping Type " + type.Name);
            

                return GenType(type);
            }
           

            #endregion

            #region Dynamic

            public object CreateMapType()
            {
                GetVariablesInformation();
              
                return GenType(null);
            }

            private void GetVariablesInformation()
            {

                foreach (var cd in program.GetUniformDescriptions())
                {
                    VarInfo v;

                    if (!_varlookup.TryGetValue(cd.Name, out v))
                    {
                        v = new VarInfo()
                        {
                            Desc = cd,
                            Name = cd.Name,                       
                            NetType = ResolveType(cd),
                        };

                        if (v.NetType != null)
                        {
                            v.Variable = CreateVariable(v.NetType, v.Desc);
                            v.Variable.Name = cd.Name;
                            v.Variable.Binder = program.CreateUniformSetter(cd.Name);     

                            if (v.Variable is ShaderVariableArray && ((ShaderVariableArray)v.Variable).Elements != cd.Type.Elements)
                            {
                                throw new InvalidOperationException("There are two array with the same name \"" + cd.Name + "\" but diferent dimensions in the effect");
                            }
                        }

                        _varlookup.Add(cd.Name, v);
                    }                  
                }             
            }

            private Type ResolveType(ShaderReflectionVariable cd)
            {
                var type = _ResolveType(cd);

                if (type != null && cd.Type.Elements > 1)
                    return type.MakeArrayType();

                return type;
            }

            private Type _ResolveType(ShaderReflectionVariable cd)
            {
                var type = cd.Type;
                switch (type.Class)
                {
                    case TypeClass.Scalar:
                        {
                            switch (type.Type)
                            {
                                case ShaderType.UserDefined: throw new InvalidOperationException();
                                case ShaderType.Bool: return typeof(bool);
                                case ShaderType.Int: return typeof(int);
                                case ShaderType.Float: return typeof(float);
                            }
                        }
                        break;
                    case TypeClass.Vector:
                        if (type.Columns == 2)
                            return typeof(Vector2);
                        else if (type.Columns == 3)
                            return typeof(Vector3);
                        else if (type.Type == ShaderType.Int)
                            return typeof(Int4);
                        else return typeof(Vector4);
                    case TypeClass.Matrix: return typeof(Matrix);
                    case TypeClass.Object:
                        {
                            switch (type.Type)
                            {
                                case ShaderType.Texture:
                                    return typeof(Texture);
                                case ShaderType.Texture1D:
                                    return typeof(Texture1D);
                                case ShaderType.Texture2D:
                                    return typeof(Texture2D);
                                case ShaderType.Texture3D:
                                    return typeof(Texture3D);
                                case ShaderType.TextureCube:
                                    return typeof(Texture2D);
                                case ShaderType.Sampler:
                                case ShaderType.Sampler1D:
                                case ShaderType.Sampler2D:
                                case ShaderType.Sampler3D:
                                case ShaderType.SamplerCube:
                                    return typeof(SamplerState);
                            }
                            throw new InvalidOperationException("Type " + type.Name + " not Supported");
                        }
                    case TypeClass.Struct: return null;
                }
                return null;
            }

            #endregion
        }

        class VarInfo
        {
            public string Name;
            public ShaderVariable Variable;
            public Type NetType;
            public PropertyInfo NetProp;            
            public ShaderReflectionVariable Desc;

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion
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
