using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Importers.FBX
{
    public enum ContentType { FLOAT, STRING , FLOAT_LIST, STRING_LIST, MIXED_LIST}

    public class FBXProperty : FBXDeclarationNode 
    {
        public string Text { get; set; }

        public ContentType ContentType { get; set; }
    }
    
    public class FBXListProperty : FBXProperty
    {
        public FBXListProperty() { }
        
        public FBXListProperty(List<string>values ,ContentType type)
        {
            if (values != null)
            {
                if (type == FBX.ContentType.STRING_LIST || type == FBX.ContentType.MIXED_LIST)
                {
                    Values = new List<string>(values.Select(x =>
                       {
                           if (x[0] == '\"')
                               return x.Substring(1, x.Length - 2);
                           return x;
                       }));
                }
                else
                {
                    Values = values;
                }
            }
            else
                Values = new List<string>(); 
        }

        public List<string> Values { get; set; }
    }

    public class FBXFloatListProperty : FBXProperty
    {
        public FBXFloatListProperty(List<string>values)
        {            
            ContentType = FBX.ContentType.FLOAT_LIST;
            if (values != null)
            {
                FloatValues = new List<float>(values.Select(x => float.Parse(x)));
            }
            else
                FloatValues = new List<float>();
        }

        public List<float> FloatValues { get; set; }
    }

    //public class FBXObjectProperty : FBXListProperty
    //{
    //    public FBXObjectProperty(FBXListProperty rawProperty)
    //    {
    //        Type = rawProperty.Type;
    //        Text = rawProperty.Text;
    //        Values = rawProperty.Values;

    //        Name = Values[0];
    //        Type = Values[1];
    //    }

    //    public string Name { get; set; }        
    //}
    
}
