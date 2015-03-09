using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Igneel.Graphics
{
    public interface IShaderRepository
    {
        /// <summary>
        /// returns the global path for the given shaderfilename
        /// </summary>
        /// <param name="shaderFilename">filename whitout extension</param>
        /// <returns></returns>
        string Locate(string shaderFilename);       

        /// <summary>
        /// Shader compilation flags
        /// </summary>
        ShaderFlags CompilerFlags { get; }

        /// <summary>
        /// Shader model used
        /// </summary>
        string ShaderModel { get; }

        string[] ShaderIncludePaths { get; set; }
    }

    public class ShaderRepository:IShaderRepository
    {
        string rootDirectory;
        string[] includePaths;
        ShaderFlags compilerFlags;
        string shaderModel;

        private ShaderRepository(string directory, ShaderFlags compilerFlags, string shaderModel)
        {
            rootDirectory = directory;
            this.compilerFlags = compilerFlags;
            this.shaderModel = shaderModel;
        }


        public string[] ShaderIncludePaths
        {
            get { return includePaths; }
            set { includePaths = value; }
        }

        public string Locate(string shaderFilename)
        {
            string s = Locate(rootDirectory, shaderFilename);
            if (s == null)
                throw new FileNotFoundException(shaderFilename);
            return s;
        }

        string Locate(string directory, string file)
        {
            foreach (var filename in Directory.EnumerateFiles(directory, file + ".*" ))
            {
                return filename;
            }
            foreach (var dir in Directory.EnumerateDirectories(directory))
            {
                string s = Locate(dir, file);
                if (s != null)
                    return s;
            }
            return null;
        }

        public ShaderFlags CompilerFlags
        {
            get { return compilerFlags; }
        }

        public string ShaderModel
        {
            get { return shaderModel; }
        }

        public static IShaderRepository SetupD3D10_SM40(string dir = "../../../Shaders/SM40/")
        {
            var rep = new ShaderRepository(dir, ShaderFlags.PackMatrixRowMajor | ShaderFlags.OptimizationLevel3, "4_0");
            Service.Set<IShaderRepository>(rep);
            return rep;
        }
      
        public static IShaderRepository SetupD3D9_SM30(string dir = "../../../Shaders/SM30/")
        {
            var rep = new ShaderRepository(dir, ShaderFlags.PackMatrixRowMajor|ShaderFlags.OptimizationLevel3, "3_0");
            Service.Set<IShaderRepository>(rep);
            return rep;
        }

        public void AddIncludeFiles(string directory = null)
        {
            if (directory == null)
                directory = rootDirectory;
            List<string> includePaths = new List<string>(10);
            DirectoryInfo di = new DirectoryInfo(directory);
            GenerateIncludeFiles(di, includePaths);
            this.includePaths = includePaths.ToArray();
        }

        private void GenerateIncludeFiles(DirectoryInfo di, List<string> includePaths)
        {
            if (ContainsShaderCodes(di))
                includePaths.Add(di.FullName);

            foreach (var item in di.EnumerateDirectories())
            {            
                GenerateIncludeFiles(item, includePaths);
            }
        }

        private bool ContainsShaderCodes(DirectoryInfo di)
        {
            return di.EnumerateFiles().Count(x => x.Extension == ".hlsli" || x.Extension == ".hlsl") > 0;
        }
        
    }    
}
