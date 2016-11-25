using Igneel.Compiling.Declarations;
using Igneel.Compiling.Runtime;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling
{
    public class ShaderRuntime
    {
        public static readonly TypeDeclaration Boolean = new BoolType();

        public static readonly TypeDeclaration Int = new IntType();

        public static readonly TypeDeclaration Float = new FloatType();

        public static readonly TypeDeclaration String = new StringType();

        public static readonly TypeDeclaration Void = new PrimitiveTypeDeclaration("void");

        public static readonly TypeDeclaration Unknow = new UnknowTypeDeclaration();

        public static readonly TypeDeclaration SamplerComparisonState = new SamplerType("SamplerComparisonState");

        public static readonly TypeDeclaration SamplerState = new SamplerType("SamplerState");


        public static void AddRuntimeTypes(Scope scope)
        {
            scope.AddType(Boolean);
            scope.AddType(Int);
            scope.AddType(Float);
            scope.AddType(String);
            scope.AddType(Void);

            AddVectorTypes(scope);
            AddMatrixTypes(scope);
            AddSamplers(scope);
            AddTextureTypes(scope);           
            AddFuncions(scope);
        }

        private static void AddFuncions(Scope scope)
        {
            var varyingAll  = new GenericArgumentType("TAll",new TypeClass[] { TypeClass.Matrix, TypeClass.Vector, TypeClass.Scalar },
                                                          new ShaderType[] { ShaderType.Bool, ShaderType.Float, ShaderType.Int });

            var varyingAllFloat = new GenericArgumentType("TFloat", new TypeClass[] { TypeClass.Matrix, TypeClass.Vector, TypeClass.Scalar },
                                                                   new ShaderType[] { ShaderType.Float});
            var varyingAllInt = new GenericArgumentType("TInt",new TypeClass[] { TypeClass.Matrix, TypeClass.Vector, TypeClass.Scalar },
                                                                new ShaderType[] { ShaderType.Int });
            var varyingAllBool = new GenericArgumentType("TBool",new TypeClass[] { TypeClass.Matrix, TypeClass.Vector, TypeClass.Scalar },
                               new ShaderType[] { ShaderType.Bool });

            var varyingAllFloatInt = new GenericArgumentType("TFloatInt",new TypeClass[] { TypeClass.Matrix, TypeClass.Vector, TypeClass.Scalar },
                               new ShaderType[] { ShaderType.Int, ShaderType.Float });

            var varyingVectorFloat = new GenericArgumentType("TVectorFloat",new TypeClass[] { TypeClass.Vector },
                                                             new ShaderType[] { ShaderType.Float });
            var varyingVectorFloatInt = new GenericArgumentType("TVectorFloatInt",new TypeClass[] { TypeClass.Vector },
                                                            new ShaderType[] { ShaderType.Float , ShaderType.Int});

            var varyingMatrix = new GenericArgumentType("Matrix", new TypeClass[] { TypeClass.Matrix },
                          new ShaderType[] { ShaderType.Float, ShaderType.Int });

            #region Math Functions            
            scope.AddFunction(new GenericFunctionDefinition("acos", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("asin", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("sin", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("cos", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("tan", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("tanh", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("atan", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("atan2", varyingAllFloat, 
                new ParameterDeclaration(varyingAllFloat, "x", 0),
                new ParameterDeclaration(varyingAllFloat, "y", 1))
                .WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("ceil", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("atan2", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("cosh", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("exp", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("exp2", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("floor", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("fmod", varyingAllFloat, 
                new ParameterDeclaration(varyingAllFloat, "x", 0),
                new ParameterDeclaration(varyingAllFloat, "y", 1)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("frac", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("frexp", varyingAllFloat, 
                new ParameterDeclaration(varyingAllFloat, "x", 0),
                new ParameterDeclaration(varyingAllFloat, "exp", 1)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("ldexp", varyingAllFloat, 
                new ParameterDeclaration(varyingAllFloat, "x", 0),
                new ParameterDeclaration(varyingAllFloat, "exp", 1)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("log", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("log10", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("log2", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("pow", varyingAllFloat, 
                new ParameterDeclaration(varyingAllFloat, "x", 0),
                new ParameterDeclaration(varyingAllFloat, "y", 1)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("radians", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("round", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("sqrt", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("rsqrt", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("saturate", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("rsqrt", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("sinh", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            #endregion

            #region Utilities
            scope.AddFunction(new GenericFunctionDefinition("abs", varyingAllFloatInt, new ParameterDeclaration(varyingAllFloatInt, "x", 0)).WithArgs(varyingAllFloatInt));
            scope.AddFunction(new GenericFunctionDefinition("all", Boolean, new ParameterDeclaration(varyingAll, "x", 0)).WithArgs(varyingAll));
            scope.AddFunction(new GenericFunctionDefinition("any", Boolean, new ParameterDeclaration(varyingAll, "x", 0)).WithArgs(varyingAll));
            scope.AddFunction(new GenericFunctionDefinition("clamp", varyingAllFloatInt,
                new ParameterDeclaration(varyingAllFloatInt, "x"),
                new ParameterDeclaration(varyingAllFloatInt, "min"),
                new ParameterDeclaration(varyingAllFloatInt, "max")).WithArgs(varyingAllFloatInt));
            scope.AddFunction(new GenericFunctionDefinition("clip", Void, new ParameterDeclaration(varyingAll, "x", 0)).WithArgs(varyingAll));
            scope.AddFunction(new GenericFunctionDefinition("ddx", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("ddy", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("fwidth", varyingAllFloat, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("isfinite", Boolean, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("isnan", Boolean, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("isinf", Boolean, new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));
            scope.AddFunction(new GenericFunctionDefinition("max", varyingAllFloatInt, 
                new ParameterDeclaration(varyingAllFloatInt, "x"),
                new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));
            scope.AddFunction(new GenericFunctionDefinition("min", varyingAllFloatInt, 
                new ParameterDeclaration(varyingAllFloatInt, "x"),
                new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));
            scope.AddFunction(new GenericFunctionDefinition("noise", Float, new ParameterDeclaration(varyingVectorFloat, "x", 0)).WithArgs(varyingVectorFloat));

            scope.AddFunction(new GenericFunctionDefinition("lerp", varyingAllFloat, 
                  new ParameterDeclaration(varyingAllFloat, "x", 0),
                  new ParameterDeclaration(varyingAllFloat, "y", 0),
                  new ParameterDeclaration(Float, "s", 0)).WithArgs(varyingAllFloat));

            scope.AddFunction(new GenericFunctionDefinition("smoothstep", varyingAllFloat,
                 new ParameterDeclaration(varyingAllFloat, "min", 0),
                 new ParameterDeclaration(varyingAllFloat, "max", 0),
                 new ParameterDeclaration(Float, "x", 0)).WithArgs(varyingAllFloat));

            scope.AddFunction(new GenericFunctionDefinition("step", varyingAllFloat,
               new ParameterDeclaration(varyingAllFloat, "y", 0),
               new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));

            scope.AddFunction(new GenericFunctionDefinition("trunc", varyingAllFloat,
                new ParameterDeclaration(varyingAllFloat, "x", 0)).WithArgs(varyingAllFloat));

            scope.AddFunction(new StdFunctionDeclaration("lit", scope.GetType("float4"), 
                new ParameterDeclaration(Float, "n_dot_l"),
                new ParameterDeclaration(Float, "n_dot_h"),
                new ParameterDeclaration(Float, "m")));

            #endregion

            #region Vector Functions
            scope.AddFunction(new GenericFunctionDefinition("dot", Float, 
                new ParameterDeclaration(varyingVectorFloatInt, "x"),
                new ParameterDeclaration(varyingVectorFloatInt, "y")).WithArgs(varyingVectorFloatInt));

            scope.AddFunction(new StdFunctionDeclaration("cross", scope.GetType("float3"), 
                new ParameterDeclaration(scope.GetType("float3"), "x"), 
                new ParameterDeclaration(scope.GetType("float3"), "y")));

            scope.AddFunction(new GenericFunctionDefinition("length", Float,
                new ParameterDeclaration(varyingVectorFloat, "x")).WithArgs(varyingVectorFloat));

            scope.AddFunction(new GenericFunctionDefinition("reflect", varyingVectorFloat, 
                new ParameterDeclaration(varyingVectorFloat, "i"),
                new ParameterDeclaration(varyingVectorFloat, "n")).WithArgs(varyingVectorFloat));

            scope.AddFunction(new GenericFunctionDefinition("refract", varyingVectorFloat, 
                new ParameterDeclaration(varyingVectorFloat, "i"), 
                new ParameterDeclaration(varyingVectorFloat, "n"),
                new ParameterDeclaration(Float, "refractionIndex")).WithArgs(varyingVectorFloat));

            scope.AddFunction(new GenericFunctionDefinition("distance", Float,
                new ParameterDeclaration(varyingVectorFloat, "x"),
                new ParameterDeclaration(varyingVectorFloat, "y")).WithArgs(varyingVectorFloat));

            scope.AddFunction(new GenericFunctionDefinition("normalize", varyingVectorFloat,
                new ParameterDeclaration(varyingVectorFloat, "x")).WithArgs(varyingVectorFloat));        
          
            #endregion

            #region Matrix Functions           

            scope.AddFunction(new GenericFunctionDefinition("transpose", varyingMatrix,
                new ParameterDeclaration(varyingMatrix, "x")).WithArgs(varyingMatrix));

            scope.AddFunction(new GenericFunctionDefinition("mul", varyingMatrix, 
                new ParameterDeclaration(varyingMatrix, "x"),
                new ParameterDeclaration(varyingMatrix, "y")).WithArgs(varyingMatrix));

            scope.AddFunction(new StdFunctionDeclaration("mul", scope.GetType("float2"),
                new ParameterDeclaration(scope.GetType("float2"), "x"),
                new ParameterDeclaration(scope.GetType("float2x2"), "y")));

            scope.AddFunction(new StdFunctionDeclaration("mul", scope.GetType("float3"),
             new ParameterDeclaration(scope.GetType("float3"), "x"),
             new ParameterDeclaration(scope.GetType("float3x3"), "y")));

            scope.AddFunction(new StdFunctionDeclaration("mul", scope.GetType("float4"),
                new ParameterDeclaration(scope.GetType("float4"), "x"),
                new ParameterDeclaration(scope.GetType("float4x4"), "y")));

            //scope.AddFunction(new StdFunctionDeclaration("mul", scope.GetType("float"),
            //    new ParameterDeclaration(scope.GetType("float3"), "x"),
            //    new ParameterDeclaration(scope.GetType("float3"), "y")));

            //scope.AddFunction(new StdFunctionDeclaration("mul", scope.GetType("float"),
            //    new ParameterDeclaration(scope.GetType("float4"), "x"),
            //    new ParameterDeclaration(scope.GetType("float4"), "y")));

            //scope.AddFunction(new StdFunctionDeclaration("mul", scope.GetType("float"),
            //    new ParameterDeclaration(scope.GetType("float"), "x"),
            //    new ParameterDeclaration(scope.GetType("float"), "y")));

            #endregion

            #region Aritmetic Operators

            scope.AddFunction(new GenericFunctionDefinition("+", varyingAllFloatInt,
                new ParameterDeclaration(varyingAllFloatInt, "x"),
                new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));

            scope.AddFunction(new GenericFunctionDefinition("-", varyingAllFloatInt,
                new ParameterDeclaration(varyingAllFloatInt, "x"),
                new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));

            scope.AddFunction(new GenericFunctionDefinition("*", varyingAllFloatInt,
                new ParameterDeclaration(varyingAllFloatInt, "x"),
                new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));

            scope.AddFunction(new GenericFunctionDefinition("/", varyingAllFloatInt,
                new ParameterDeclaration(varyingAllFloatInt, "x"),
                new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));

            scope.AddFunction(new GenericFunctionDefinition("-", varyingAllFloatInt,
              new ParameterDeclaration(varyingAllFloatInt, "x")).WithArgs(varyingAllFloatInt));

            #endregion

            #region Relational Operators
            var matchType = new DimentionMatchType("bool", scope);

            scope.AddFunction(new GenericFunctionDefinition("<", matchType,
               new ParameterDeclaration(varyingAllFloatInt, "x"),
               new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));

            scope.AddFunction(new GenericFunctionDefinition(">", matchType,
                new ParameterDeclaration(varyingAllFloatInt, "x"),
                new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));

            scope.AddFunction(new GenericFunctionDefinition("<=", matchType,
                new ParameterDeclaration(varyingAllFloatInt, "x"),
                new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));

            scope.AddFunction(new GenericFunctionDefinition(">=", matchType,
                new ParameterDeclaration(varyingAllFloatInt, "x"),
                new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));

            scope.AddFunction(new GenericFunctionDefinition("==", matchType,
              new ParameterDeclaration(varyingAllFloatInt, "x"),
              new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));

            scope.AddFunction(new GenericFunctionDefinition("!=", matchType,
               new ParameterDeclaration(varyingAllFloatInt, "x"),
               new ParameterDeclaration(varyingAllFloatInt, "y")).WithArgs(varyingAllFloatInt));
          
            #endregion

            #region Logical Operators

            scope.AddFunction(new GenericFunctionDefinition("!", matchType,
            new ParameterDeclaration(varyingAllBool, "x")).WithArgs(varyingAllBool));


            scope.AddFunction(new GenericFunctionDefinition("&&", varyingAllBool,
            new ParameterDeclaration(varyingAllBool, "x"),
            new ParameterDeclaration(varyingAllBool, "y")).WithArgs(varyingAllBool));

            scope.AddFunction(new GenericFunctionDefinition("||", varyingAllBool,
                new ParameterDeclaration(varyingAllBool, "x"),
                new ParameterDeclaration(varyingAllBool, "y")).WithArgs(varyingAllBool));

            scope.AddFunction(new GenericFunctionDefinition("^", varyingAllBool,
                new ParameterDeclaration(varyingAllBool, "x"),
                new ParameterDeclaration(varyingAllBool, "y")).WithArgs(varyingAllBool));

            #endregion
        }

        private static  void AddSamplers(Scope scope)
        {
            //SamplerState.Members = new MemberDeclaration[]
            //{
            //    new MemberDeclaration{Name = "AddressU", Type = Int, DeclaringType = samplerState},
            //    new MemberDeclaration{Name = "AddressV", Type = Int,DeclaringType = samplerState},
            //    new MemberDeclaration{Name = "AddressW", Type = Int,DeclaringType = samplerState},
            //    new MemberDeclaration{Name = "Filter", Type = Int,DeclaringType = samplerState},
            //    new MemberDeclaration{Name = "MaxAnisotropy", Type = Int,DeclaringType = samplerState},
            //    new MemberDeclaration{Name = "MaxLOD", Type = Int,DeclaringType = samplerState},
            //    new MemberDeclaration{Name = "MinLOD", Type = Int,DeclaringType = samplerState},
            //    new MemberDeclaration{Name = "MipLODBias", Type = Int,DeclaringType = samplerState},
            //    new MemberDeclaration{Name = "ComparisonFunc", Type = Int,DeclaringType = samplerState},
            //    new MemberDeclaration{Name = "ComparisonFilter ", Type = Int,DeclaringType = samplerState},
            //};
            scope.AddType(SamplerState);

            //var samplerCmpState = new PrimitiveTypeDeclaration("SamplerComparisonState");
            //samplerCmpState.Members = new MemberDeclaration[]
            //{
            //    new MemberDeclaration{Name = "AddressU", Type = Int, DeclaringType = samplerCmpState},
            //    new MemberDeclaration{Name = "AddressV", Type = Int,DeclaringType = samplerCmpState},
            //    new MemberDeclaration{Name = "AddressW", Type = Int,DeclaringType = samplerCmpState},
            //    new MemberDeclaration{Name = "Filter", Type = Int,DeclaringType = samplerCmpState},
            //    new MemberDeclaration{Name = "MaxAnisotropy", Type = Int,DeclaringType = samplerCmpState},
            //    new MemberDeclaration{Name = "MaxLOD", Type = Int,DeclaringType = samplerCmpState},
            //    new MemberDeclaration{Name = "MinLOD", Type = Int,DeclaringType = samplerCmpState},
            //    new MemberDeclaration{Name = "MipLODBias", Type = Int,DeclaringType = samplerCmpState},
            //    new MemberDeclaration{Name = "ComparisonFunc", Type = Int,DeclaringType = samplerCmpState},
            //    new MemberDeclaration{Name = "ComparisonFilter ", Type = Int,DeclaringType = samplerCmpState},
            //};
            scope.AddType(SamplerComparisonState);       

        }

        private static  void AddTextureTypes(Scope scope)
        {
            scope.AddType(new PrimitiveTypeDeclaration("Texture2D")
             {
                 Methods = new FunctionDeclaration[]
                {
                     new StdFunctionDeclaration("Sample",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float2"), "location",1), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",2){ Opcional = true}),
                    new StdFunctionDeclaration("SampleLevel",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float2"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "lod",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                    new StdFunctionDeclaration("SampleBias",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float2"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "bias",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                     new StdFunctionDeclaration("SampleCmp",scope.GetType("float"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float2"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "compareValue",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                     new StdFunctionDeclaration("SampleCmpLevelZero",scope.GetType("float"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float2"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "compareValue",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                     new StdFunctionDeclaration("GetDimensions", Void, 
                         new ParameterDeclaration(Float, "width",0),
                         new ParameterDeclaration(Float, "height",1))
                }
             });

            scope.AddType(new PrimitiveTypeDeclaration("Texture1D")
            {
                Methods = new FunctionDeclaration[]
                {
                     new StdFunctionDeclaration("Sample",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float"), "location",1), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",2){ Opcional = true}),
                    new StdFunctionDeclaration("SampleLevel",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "lod",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                    new StdFunctionDeclaration("SampleBias",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "bias",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                     new StdFunctionDeclaration("SampleCmp",scope.GetType("float"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "compareValue",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                     new StdFunctionDeclaration("SampleCmpLevelZero",scope.GetType("float"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "compareValue",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true})
                }
            });

            scope.AddType(new PrimitiveTypeDeclaration("Texture3D")
            {
                Methods = new FunctionDeclaration[]
                {
                     new StdFunctionDeclaration("Sample",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",2){ Opcional = true}),
                    new StdFunctionDeclaration("SampleLevel",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "lod",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                    new StdFunctionDeclaration("SampleBias",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "bias",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true})                   
                }
            });

            scope.AddType(new PrimitiveTypeDeclaration("TextureCube")
            {
                Methods = new FunctionDeclaration[]
                {
                     new StdFunctionDeclaration("Sample",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",2){ Opcional = true}),
                    new StdFunctionDeclaration("SampleLevel",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "lod",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                    new StdFunctionDeclaration("SampleBias",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "bias",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                     new StdFunctionDeclaration("SampleCmp",scope.GetType("float"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "compareValue",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                     new StdFunctionDeclaration("SampleCmpLevelZero",scope.GetType("float"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "compareValue",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true})
                }
            });

            scope.AddType(new PrimitiveTypeDeclaration("Texture2DArray")
            {
                Methods = new FunctionDeclaration[]
                {
                     new StdFunctionDeclaration("Sample",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",2){ Opcional = true}),
                    new StdFunctionDeclaration("SampleLevel",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "lod",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                    new StdFunctionDeclaration("SampleBias",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "bias",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                     new StdFunctionDeclaration("SampleCmp",scope.GetType("float"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "compareValue",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                     new StdFunctionDeclaration("SampleCmpLevelZero",scope.GetType("float"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float3"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "compareValue",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true})
                }
            });

            scope.AddType(new PrimitiveTypeDeclaration("Texture1DArray")
            {
                Methods = new FunctionDeclaration[]
                {
                     new StdFunctionDeclaration("Sample",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float2"), "location",1), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",2){ Opcional = true}),
                    new StdFunctionDeclaration("SampleLevel",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float2"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "lod",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                    new StdFunctionDeclaration("SampleBias",scope.GetType("float4"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float2"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "bias",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                     new StdFunctionDeclaration("SampleCmp",scope.GetType("float"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float2"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "compareValue",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true}),
                     new StdFunctionDeclaration("SampleCmpLevelZero",scope.GetType("float"), 
                        new ParameterDeclaration(scope.GetType("SamplerState"), "sampler",0), 
                        new ParameterDeclaration(scope.GetType("float2"), "location",1), 
                        new ParameterDeclaration(scope.GetType("float"), "compareValue",2), 
                        new ParameterDeclaration(scope.GetType("int2"), "offset",3){ Opcional = true})
                }
            });
        }
    
        private static void AddVectorTypes(Scope scope)
        {
            string[] names = new string[] { "float", "int", "bool" };

            Func<Scope, string, PrimitiveTypeDeclaration>[] activors = 
            {
                (s, name)=>new Vector2Type(scope,name),
                (s,name)=>new Vector3Type(scope,name),
                (s,name)=>new Vector4Type(scope,name),
            };

            for (int i = 0; i < names.Length; i++)
            {
                for (int column = 0; column < 3; column++)
                {
                    var vectorType = activors[column](scope, names[i]);
                    scope.AddType(vectorType);

                }  
            }
        }

        private static void AddMatrixTypes(Scope scope)
        {         
            TypeDeclaration[] memberTypes = { Float, Int, Boolean };
            string[] scalarTypes = { Float.Name, Int.Name, Boolean.Name };
            Graphics.ShaderType[] types = { Graphics.ShaderType.Float, Graphics.ShaderType.Int, Graphics.ShaderType.Bool };          

            for (int i = 0; i < scalarTypes.Length; i++)
            {
                for (int row = 0; row < 3; row++)
                    for (int column = 0; column < 3; column++)
                    {
                        var name = memberTypes[i].Name + (column + 2).ToString() + "x" + (row + 2).ToString();
                        var matrixType = new PrimitiveTypeDeclaration(name)
                        {                           
                            Colums = column + 2,
                            Size = 4 * ((column + 2) * (row + 2)),
                            ReflectionType = new Graphics.ShaderReflectionType()
                            {
                                Name =name,
                                Class = Graphics.TypeClass.Matrix,
                                Columns = column + 2,
                                Rows = row + 2,
                                Elements = 1,
                                Register = Graphics.RegisterSet.Float4,
                                Type = types[i]
                            }
                        };
                        var members = new MemberDeclaration[matrixType.Colums * matrixType.Rows];
                        matrixType.Members = members;
                        for (int r = 0; r < matrixType.Rows; r++)
                            for (int c = 0; c < matrixType.Colums; c++)
                            {
                                members[r * 4 + c] = new MemberDeclaration
                                {
                                    Name = "_"+(r+1).ToString()+(c+1).ToString(),
                                    DeclaringType = matrixType,
                                    Offset = 4 * (r * 4 + c),
                                    Register = Graphics.RegisterSet.Float4,
                                    Type = memberTypes[i]
                                };
                            }

                        scope.AddType(matrixType);

                    }
            }
        }

        public static Graphics.ShaderType GetShaderType(string name)
        {
            switch (name)
            {
                case "float": return ShaderType.Float;
                case "int": return ShaderType.Int;
                case "bool": return ShaderType.Bool;
                default: return ShaderType.Unsupported;
            }
        }

        public static MemberDeclaration DeclareMember(TypeDeclaration type, string name, int index, TypeDeclaration scalarType)
        {
            return new MemberDeclaration
            {
                Name = name,
                DeclaringType = type,
                Offset = scalarType.Size * index,
                Register = Graphics.RegisterSet.Float4,
                Type = scalarType
            };
        }
    }
}
