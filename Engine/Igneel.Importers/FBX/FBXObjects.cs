using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Importers.FBX
{
    public class FBXObject : FBXDeclarationNode, IFBXContainer
    {
        SortedList<string, List<FBXDeclarationNode>> declarations;

        public FBXObject()
        {
            declarations = new SortedList<string, List<FBXDeclarationNode>>();
        }

        public FBXObject(string id, string type)
        {
            if (id != null)
            {
                id = id.Substring(1, id.Length - 2);
                string[] s = id.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length == 2)
                {
                    InstanceType = s[0];
                    Name = s[1];
                }
                else
                    Name = s[0];
                Id = id;
            }
            if (type != null)
            {
                type = type.Substring(1, type.Length - 2);
                RelatedType = type;
            }                        
            if (type == null)
                IsReference = true;
            declarations = new SortedList<string, List<FBXDeclarationNode>>();
        }        

        public string Name { get; set; }

        public string Id { get; set; }

        public string InstanceType { get; set; }

        public string RelatedType { get; set; }

        public bool IsReference { get; private set; }

        public int Index { get; set; }

        public SortedList<string, List<FBXDeclarationNode>> Declarations { get { return declarations; } set { declarations = value; } }

        public void Add(FBXDeclarationNode node)
        {
            List<FBXDeclarationNode>list;
            if (!declarations.TryGetValue(node.Type, out list))
            {
                list = new List<FBXDeclarationNode> { node };
                declarations.Add(node.Type, list);
            }
            else
                list.Add(node);
        }

        public T GetDeclaration<T>(string type, int index = 0) where T : FBXDeclarationNode
        {
            List<FBXDeclarationNode> values;
            if (declarations.TryGetValue(type,out values) && index < values.Count)
                return (T)values[index];
            return null;
        }

        public FBXObject GetObject(string type, int index = 0)
        {
            return GetDeclaration<FBXObject>(type, index);
        }

        public FBXListProperty GetProperty(string name)
        {
            FBXObject propertys = (FBXObject)declarations["Properties60"][0];
            return (FBXListProperty)propertys.declarations["Property"].Find(x => ((FBXListProperty)x).Values[0] == name);
        }

        public FBXObject GetObjectById(string id, string type = null)
        {
            if (type == null)
            {
                foreach (var l in declarations.Values)
                {
                    for (int i = 0; i < l.Count; i++)
                    {
                        FBXObject obj = l[i] as FBXObject;
                        if (obj != null && obj.Id == id)
                            return obj;
                    }
                }
            }
            else
            {
                List<FBXDeclarationNode> list;
                if (!declarations.TryGetValue(type, out list))
                    return null;

                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        FBXObject obj = list[i] as FBXObject;
                        if (obj != null && obj.Id == id)
                            return obj;
                    }
                }
            }           
            return null;
        }

        public int DeclarationCount(string type)
        {
              List<FBXDeclarationNode> values;
              if (declarations.TryGetValue(type, out values))
                  return values.Count;
              return 0;
        }

        public override string ToString()
        {
            string s = "\"" + Type + "\"";
            if(Id!=null)
                s+= Id;
            if (RelatedType != null)
                s += "[" + RelatedType + "]";
            return s;
        }
    }
}
