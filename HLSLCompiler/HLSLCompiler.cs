using Antlr.Runtime;
using Igneel.Compiling.Declarations;
using Igneel.Compiling.Parser;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Compiling
{
    public class HLSLCompiler
    {
        ASTNode _ast;
        List<HLSLProgram> _programs = new List<HLSLProgram>();
        Scope _runtimeScope;
        Scope _globalScope;        
        Preprocessor p;
        string dir;
        ErrorLog _log = new ErrorLog();
        string _main = "main";

        public HLSLCompiler()
        {
            _runtimeScope = new Scope();
            _runtimeScope.Log = _log;
            ShaderRuntime.AddRuntimeTypes(_runtimeScope);
            _globalScope = new Scope(_runtimeScope);            
            p = new Preprocessor();
        }

        public string[] Sources { get; set; }

        public string EntryPoint { get { return _main; } set { _main = value; } }

        public List<string> Errors { get { return _log.Errors; } }

        public string[] IncludePath { get; set; }

        public List<HLSLProgram> Programs { get { return _programs; } }


        /// <summary>
        /// Creates the HLSLProgram of all the Sources
        /// </summary>
        public void Compile()
        {
            foreach (var source in Sources)
            {               
                CompileString(source);
                if (_log.Errors.Count > 0)
                {
                    return;
                }
            }
            CheckSemantic();
        }

      
        public void CompileFile(string filename)
        {
            CompileInclude(filename);
            if (_log.Errors.Count > 0)
            {
                return;
            }
            CheckSemantic();
        }
        

        private void CompileString(string source)
        {
            var result = Preproces(source);
            CreatesAST(result);
        }

        private void CompileInclude(string filename)
        {
            string fullpath = filename;            
            for (int i = 0; i < IncludePath.Length && !File.Exists(fullpath); i++)
            {
                fullpath = Path.Combine(IncludePath[i], filename);
            }            

            if (!File.Exists(fullpath))
                throw new FileNotFoundException(filename);

            string source = File.ReadAllText(fullpath);
            CompileString(source);
        }

        private string Preproces(string source)
        {
            ANTLRStringStream stream = new ANTLRStringStream(source);
            preprocessLexer lexer = new preprocessLexer(stream);
            lexer.Preprocessor = p;
            lexer.mDOCUMENT();
            //preprocessParser parser = new preprocessParser(new CommonTokenStream(lexer));        
            //var n = parser.Doc();
            
            //StringBuilder sb = new StringBuilder();
            //foreach (var item in n)
            //{
            //    sb.Append(item.GetContent(p));   
            //}
            var text = p.ToString();         
            var includes = p.Includes.ToArray();

            p.ParseComplete();
            foreach (var item in includes)
            {
                CompileInclude(item);
            }

            return text;
        }

        private void CreatesAST(string source)
        {
            //ANTLRFileStream fStream = new ANTLRFileStream(source, Encoding.ASCII);
            ANTLRStringStream stream = new ANTLRStringStream(source);
            HLSLLexer lexer = new HLSLLexer(stream);
            HLSLParser parser = new HLSLParser(new CommonTokenStream(lexer));           
            parser.GlobalScope = _globalScope;
            var program = parser.program();

            _programs.Add(program);

            if (parser.Errors.Count > 0)
            {
                foreach (var item in parser.Errors)
                {
                    _log.Error(item.Message, item.Line, item.CharPositionInLine);
                }
                return;
            }
        }

        private void CheckSemantic()
        {
            Declaration entryPoint = null;
            foreach (var program in _programs)
            {
                foreach (var d in program.Declarations)
                {
                    d.AddToScope(_globalScope);
                    if (d.Name == _main)
                    {
                        if (entryPoint != null)
                            _log.Error("main already defined", d.Line, d.Column);
                        else
                            entryPoint = d;
                    }
                }
            }
            if (entryPoint != null)
                entryPoint.CheckSemantic(_globalScope, _log);
        }

        public string GenerateCode()
        {
            if (_log.Errors.Count > 0)
                return null;

            StringBuilder sb = new StringBuilder();
            foreach (var p in _programs)
            {
                foreach (var d in p.Declarations)
                {
                    if (d.IsUsed || d.Name == _main)
                    {
                        d.GenerateCode(sb, 0);
                        sb.AppendLine();
                    }
                }
            }

            return sb.ToString();
        }

        public static HLSLCompiler CreateASTFromString(string source, ShaderMacro[] defines)
        {
            if (source == null) throw new ArgumentNullException("source");

            var compiler = new HLSLCompiler();
            compiler.Sources = new string[] { source };
            var rep = Service.Get<IShaderRepository>();
            if (rep != null)
                compiler.IncludePath = rep.ShaderIncludePaths;
            if (defines != null)
            {
                foreach (var macro in defines)
                {
                    compiler.p.AddMacro(macro.Name, macro.Value);
                }
            }
            compiler.Compile();

            if (compiler.Errors.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var item in compiler.Errors)
                {
                    sb.AppendLine(item);
                }
                throw new ShaderCompilationException(sb.ToString());
            }
            return compiler;
        }

        public static HLSLCompiler CreateASTFromFile(string filename, ShaderMacro[] defines)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            var compiler = new HLSLCompiler();
            var rep = Service.Get<IShaderRepository>();
            if(rep!=null)
                compiler.IncludePath = rep.ShaderIncludePaths;
            if (defines != null)
            {
                foreach (var macro in defines)
                {
                    compiler.p.AddMacro(macro.Name, macro.Value);
                }
            }
            compiler.CompileFile(filename);

            if (compiler.Errors.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var item in compiler.Errors)
                {
                    sb.AppendLine(item);
                }
                throw new ShaderCompilationException(sb.ToString());                       
            }
            return compiler;
        }

        public ShaderReflection GetShaderReflection()
        {
            ShaderReflection shaderReflection = new ShaderReflection();
            int location;
            foreach (var p in _programs)
            {
                foreach (var item in p.Declarations)
                {
                    if (item is GlobalVariableDeclaration)
                    {
                        GlobalVariableDeclaration vd = (GlobalVariableDeclaration)item;
                        shaderReflection.Variables.Add(GetReflectionVariable(vd));
                    }
                    else if (item is ConstantBufferDeclaration)
                    {
                        ConstantBufferDeclaration cb = (ConstantBufferDeclaration)item;
                        BufferReflection buffer = new BufferReflection() { Type = cb.Type, Name = cb.Name };
                        foreach (var c in cb.Constants)
                        {
                            buffer.Constants.Add(GetReflectionVariable(c));
                        }
                        shaderReflection.Buffers.Add(buffer);
                    }
                }

            }
            return shaderReflection;
        }

        private static ShaderReflectionVariable GetReflectionVariable(GlobalVariableDeclaration vd)
        {
            ShaderReflectionVariable shaderVar = new ShaderReflectionVariable
            {
                Location = vd.Offset,
                Name = vd.Name,
                Semantic = vd.Semantic,
                Size = vd.Type.Size,
                Type = vd.Type.ReflectionType
            };
            return shaderVar;
        }
    }
}
