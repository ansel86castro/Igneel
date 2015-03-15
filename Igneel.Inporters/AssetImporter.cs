using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Igneel.Assets;
using System.IO;
using Igneel.Scenering;
using Igneel.Scenering.Assets;

namespace Igneel.Importers
{
    public class ImportFormatAttribute:Attribute
    {
        public ImportFormatAttribute(string fileExtension)
        {
            this.FileExtension   = fileExtension;
        }
        public string FileExtension{get;set;}
    }

    public interface IAnimationImporter
    {
        ContentPackage ImportAnimation(Scene scene, string filename);

        ContentPackage ImportAnimation(Scene scene, string filename, SceneNode root, string fileRoot);
    }

    public abstract class ContentImporter:ResourceAllocator
    {
        static Dictionary<string, Type> importerTypes = new Dictionary<string, Type>();

        public class ImportEventArg:EventArgs
        {
            public bool Cancel { get; set; }
            public ContentPackage ImportData { get; internal set; }
        }

        private bool disposed;

        public string FileName { get; set; }

        public event EventHandler<ImportEventArg> Load;

        public ContentPackage LoadFile(Scene scene, string srcfilename)
        {
            FileName = srcfilename;
            var package = _Import(scene ,srcfilename);
            if (package != null)
            {                
                if(Load!=null)
                {
                    ImportEventArg arg = new ImportEventArg() { ImportData = package };
                    Load(this, arg);
                }               
            }

            return package;
        }      

        protected abstract ContentPackage _Import(Scene scene, string filename);        
     

        public static void RegisterImporter(string extension, Type importerType)
        {
            importerTypes.Add(extension, importerType);
        }

        public static void RemoveImporter(string extension)
        {
            importerTypes.Remove(extension);
        }

        public static void InitializeDefaultLoaders()
        {
            Module mod = typeof(ContentImporter).Module;
            foreach (var type in mod.GetTypes())
            {
                var attr = (ImportFormatAttribute[])type.GetCustomAttributes(typeof(ImportFormatAttribute), true);
                if (attr.Length > 0)
                {
                    importerTypes.Add(attr[0].FileExtension, type);
                }
            }
        }

        public static ContentImporter GetImporter(string extension)
        {
            if (importerTypes.Count == 0)
                InitializeDefaultLoaders();

            var importerType = importerTypes[extension.ToLower()];
            return (ContentImporter)Activator.CreateInstance(importerType);
        }

        public static IAnimationImporter GetAnimationImporter(string extension)
        {
            if (importerTypes.Count == 0)
                InitializeDefaultLoaders();

            var importerType = importerTypes[extension.ToLower()];
            return (IAnimationImporter)Activator.CreateInstance(importerType);
        }

        public static ContentPackage Import(Scene scene, string filename)
        {
            var importer = GetImporter(Path.GetExtension(filename));
            var content = importer.LoadFile(scene, filename);
            return content;
        }

        public static ContentPackage ImportAnimation(Scene scene, string filename)
        {
            var importer = GetAnimationImporter(Path.GetExtension(filename));
            if (importer == null)
                return null;

            var content = importer.ImportAnimation(scene, filename);
            return content;
        }

        public static ContentPackage ImportAnimation(Scene scene, string filename, SceneNode root, string fileRootName = null)
        {
            var importer = GetImporter(Path.GetExtension(filename)) as IAnimationImporter;
            if (importer == null)
                return null;

            var content = importer.ImportAnimation(scene, filename, root, fileRootName);
            return content;
        }
     
    }
  
}
