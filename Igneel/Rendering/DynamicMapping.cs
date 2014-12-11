using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SArray<T>
        where T : struct
    {
        public T[] Array;
        public int Offset;
        public int Count;

        public SArray(T[] array, int offset, int count)
        {
            this.Array = array;
            this.Offset = offset;
            this.Count = count;
        }

        public SArray(T[] array, int count)
        {
            this.Array = array;
            this.Offset = 0;
            this.Count = count;
        }

        public SArray(T[] array)
        {
            this.Array = array;
            this.Offset = 0;
            this.Count = array.Length;
        }
    }

    class DynamicMapper
    {
        class VarInfo
        {
            public string Name;
            public ShaderVariable Variable;
            public Type NetType;
            public PropertyInfo NetProp;
            public List<PassInfo> Passes = new List<PassInfo>();
            public UniformDesc Desc;
            public override string ToString()
            {
                return Name;
            }
        }

        struct PassInfo
        {
            public int Technique;
            public int Pass;
            public override string ToString()
            {
                return "T:" + Technique + " P:" + Pass;
            }
        }

        internal const string BinderField = "_<cb>";
        internal static AssemblyName aName;
        internal static ModuleBuilder mb;
        internal static AssemblyBuilder assembly;
        static string _namespace;
        internal static int id;       

        static DynamicMapper()
        {
            Initialize();
        }

        public static void Initialize()
        {            
            if (aName == null)
            {
                aName = typeof(ShaderVariable).Assembly.GetName(true);
                _namespace = typeof(ShaderVariable).Namespace;
               // aName = new AssemblyName("Igneel.Graphics");
            }
            if (mb == null)
            {
                AppDomain myDomain = Thread.GetDomain();               
                assembly = myDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);                
                mb = assembly.DefineDynamicModule("DynamicModule", "DynamicModule.dll");
            }
        }

        public Effect effect;
        public EffectPass[][] techniques;

        //Dictionary<string, List<UniformDesc>[][]> fields = new Dictionary<string,List<UniformDesc>[][]>();
        //Dictionary<string, Type> varType = new Dictionary<string,Type>();
        //Dictionary<string, EffectVariable> variablesLookup = new Dictionary<string, EffectVariable>();
        
        //List<string>[][] fieldsByPass;
        //EffectVariable[] variables;        
        List<VarInfo> variables = new List<VarInfo>();
        Dictionary<string, VarInfo> varlookup = new Dictionary<string, VarInfo>();

   
        private object GenType(Type baseType)
        {
            TypeBuilder tb;
            if (baseType != null)
            {
                tb = baseType.IsInterface ?
                mb.DefineType(_namespace + "."+ "_$" + baseType.Name +id, TypeAttributes.Class | TypeAttributes.Public, typeof(object), new Type[] { baseType }) :
                mb.DefineType(_namespace + "." + "_$" + baseType.Name + id, TypeAttributes.Class | TypeAttributes.Public, baseType);
            }
            else
                tb = mb.DefineType(_namespace + "." + "$dI" + id, TypeAttributes.Class | TypeAttributes.Public);

            id++;

            var commit = typeof(ShaderVariable).GetMethod("Commit");
            List<KeyValuePair<FieldBuilder, ShaderVariable>> createFields = new List<KeyValuePair<FieldBuilder, ShaderVariable>>();            

            foreach (var v in varlookup)
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
                    var prop = baseType.GetProperty(v.Key);
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
             il.Emit(OpCodes.Ldsfld, tField);
             il.Emit(OpCodes.Ldarg_1);
             il.Emit(OpCodes.Stfld, tField.FieldType.GetField("Value"));

             // field.Commit();
             il.Emit(OpCodes.Ldsfld, tField);
             il.EmitCall(OpCodes.Call, commit, null);
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

        private ShaderVariable CreateVariable(Type type, UniformDesc desc)
        {
            if (type.IsArray)
            {
                if (desc.Elements == 0)
                    throw new InvalidOperationException("The constant "+ desc.Name +"is not an array");
                
                var e = type.GetElementType();
                if (e == typeof(int))
                    return new IntArrayVariable() {  Elements = desc.Elements };
                else if (e == typeof(bool))
                    return new BoolArrayVariable() { Elements = desc.Elements };
                else if (e == typeof(float))
                    return new FloatArrayVariable() { Elements = desc.Elements };
                else if (e == typeof(Matrix))
                    return new MatrixArrayVariable() { Elements = desc.Elements };
                else if (e == typeof(Vector4))
                    return new Vector4ArrayVariable() { Elements = desc.Elements };
                else
                {
                    var insType = typeof(ShaderVariableArray<>).MakeGenericType(e);
                    var inst = (ShaderVariableArray)Activator.CreateInstance(insType);
                    inst.Elements = desc.Elements;
                    return inst;
                }
            }
            else if (type == typeof(int))
                return new IntVariable();
            else if (type == typeof(bool))
                return new BoolVariable();
            else if (type == typeof(float))
                return new FloatVariable();
            else if (type == typeof(Matrix))
                return new MatrixVariable();
            else if (type == typeof(Vector4))
                return new Vector4Variable();
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SArray<>))
            {
                var args = type.GetGenericArguments();
                var insType = typeof(RangeVariable<>).MakeGenericType(args);
                return (ShaderVariable)Activator.CreateInstance(insType);
            }
            else
            {
                var insType = typeof(ShaderVariable<>).MakeGenericType(type);
                return (ShaderVariable)Activator.CreateInstance(insType);
            }
        }

        private string GetName(UniformDesc desc)
        {
            return desc.Elements > 0 ? desc.Name + desc.Elements : desc.Name;        
        }

        void CreateEffectVariables()
        {           
            foreach (var var in varlookup.Values)
            {
                if (var.Desc != null && var.NetType!=null)
                {
                    var.Variable = effect.GetVariable(var.Name);                    
                    if (var.Variable == null)
                    {
                        var.Variable = CreateVariable(var.NetType, var.Desc);
                        var.Variable.Name = var.Name;
                        effect.SetVariable(var.Name, var.Variable);
                    }
                }
            }
        }

        void RegisterVariables()
        {
            foreach (var v in varlookup.Values)
            {
                foreach (var pass in v.Passes)
	            {
		            techniques[pass.Technique][pass.Pass].AddVariable(v.Name, v.Variable);
	            }
            }

        }

        #region DynamicMapping

        public object CreateMapType()
        {                                 
            GetVariablesInformation();

            CreateEffectVariables();

            RegisterVariables();

            return GenType(null);
        }       

        private void GetVariablesInformation()
        {
            for (int i = 0; i < techniques.Length; i++)
            {
                for (int j = 0; j < techniques[i].Length; j++)
                {
                    foreach (var cd in techniques[i][j].Program.GetUniformDescriptions())
                    {
                        VarInfo v;
                       
                        if (!varlookup.TryGetValue(cd.Name, out v))
                        {
                            v = new VarInfo()
                            {
                                Desc = cd,
                                Name = cd.Name,
                                NetType = ResolveType(cd),
                            };
                            varlookup.Add(cd.Name, v);
                        }
                        var effectVariable = effect.GetVariable(cd.Name);
                        if (effectVariable == null)
                        {
                            v.Passes.Add(new PassInfo { Technique = i, Pass = j });
                        }
                        else if(effectVariable is ShaderVariableArray && 
                            ((ShaderVariableArray)effectVariable).Elements != cd.Elements)
                        {
                            throw new InvalidOperationException("There are two array with the same name \""+cd.Name+"\" but diferent dimensions in the effect");
                        }
                    }
                }
            }

       
        }

        private Type ResolveType(UniformDesc cd)
        {
            var type = _ResolveType(cd);            

            if (type !=null && cd.Elements > 1)
                return type.MakeArrayType();            

            return type;
        }

        private Type _ResolveType(UniformDesc cd)
        {
            switch (cd.Class)
            {
                case ParameterClass.SCALAR:
                    {
                        switch (cd.Type)
                        {
                            case ParameterType.VOID: throw new InvalidOperationException();                                
                            case ParameterType.BOOL: return typeof(bool);
                            case ParameterType.INT: return typeof(int);
                            case ParameterType.FLOAT: return typeof(float);                            
                        }
                    }
                    break;
                case ParameterClass.VECTOR:
                    if(cd.Columns == 2)
                        return typeof(Vector2);
                    else if(cd.Columns == 3)
                        return typeof(Vector3);
                    else if(cd.Type == ParameterType.INT)
                        return typeof(Int4);
                    else return typeof(Vector4);                
                case ParameterClass.MATRIX: return typeof(Matrix);
                case ParameterClass.OBJECT: return null;
                case ParameterClass.STRUCT: return  CreateStruct(cd);                                                                                           
            }
            return null;
        }

        private Type CreateStruct(UniformDesc cd)
        {
            var shaderType = ShaderType.GetShaderType(cd);
            if (shaderType != null)
                return shaderType.NetType;
            return null;           
        }            

        #endregion

        #region Static Mapping

        PropertyInfo[] GetProperties(Type type)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();
            var interfaces = type.GetInterfaces();
            if (interfaces.Length > 0)
            {
                foreach (var item in interfaces)
                {
                    var props = GetProperties(item);
                    properties.AddRange(props);
                }
            }

            properties.AddRange(type.GetProperties());
            return properties.ToArray();
        }

        public object CreateMapType(Type type, bool fullMapping = false)
        {          
            var props = GetProperties(type);            
            
            GetVariablesInformation(props);

            if (varlookup.Values.All(x => x.Desc == null))
                return null;

            if (fullMapping && varlookup.Count < props.Length)
                throw new InvalidMappingException("Invalid Mapping Type " + type.Name);

            CreateEffectVariables();

            RegisterVariables();

            return GenType(type);
        }

        private void GetVariablesInformation(PropertyInfo[] props)
        {
            foreach (var p in props)
            {
                for (int i = 0; i < techniques.Length; i++)
                {
                    for (int j = 0; j < techniques[i].Length; j++)
                    {
                        try
                        {
                            VarInfo v;
                            UniformDesc ud = techniques[i][j].Program.GetUniformDescription(p.Name);

                            if (!varlookup.TryGetValue(p.Name, out v))
                            {
                                v = new VarInfo()
                                {
                                    Name = p.Name,                                    
                                    NetProp = p,
                                };
                                varlookup.Add(v.Name, v);
                            }
                            if (v.Desc == null)
                            {
                                //Check is v.Desc is equal ud
                                v.Desc = ud;
                                v.NetType = ud != null ? p.PropertyType : null;
                            }

                            if (ud != null && !techniques[i][j].ContainsVariable(v.Name))
                                v.Passes.Add(new PassInfo { Technique = i, Pass = j });
                        }
                        catch (NullReferenceException e)
                        {

                        }
                    }
                }
            }                        
      
        }             
     
        #endregion
    }   
}
