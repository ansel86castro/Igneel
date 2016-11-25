using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
namespace Igneel.Importers
{
    public class XFileParser
    {      
        public static void Parse(string filename, object userData,Func<XFileData, XFileData, XFile,object,bool> ParseDataObject)
        {
            XFile xfile=null;
            XFileEnumerationObject firstObject=null;
            try
            {
                xfile = new XFile();
                
                xfile.RegisterTemplates(XFile.DefaultTemplates);
                //xfile.RegisterTemplates(XFile.ExtensionTemplates);
                xfile.RegisterTemplates(XFile.SkinTemplates);

                firstObject = xfile.CreateEnumerationObject(filename, System.Runtime.InteropServices.CharSet.Ansi);
                for (int i = 0; i < firstObject.ChildCount; i++)
                {
                    if (!ParseDataObject(firstObject.GetChild(i), null, xfile, userData))
                        break;
                }
            }
            finally
            {
                if(firstObject!=null)
                firstObject.Dispose();
                if(xfile!=null)
                xfile.Dispose();
            }
        }            
        
    }
}
