using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;

namespace Igneel.Importers.FBX
{
    public interface IFBXContainer
    {
        SortedList<string, List<FBXDeclarationNode>> Declarations { get; set; }
         void Add(FBXDeclarationNode node);
         T GetDeclaration<T>(string type, int index = 0) where T : FBXDeclarationNode;
         FBXObject GetObject(string type, int index = 0);

    }
    public class FBXDocument:IFBXContainer
    {
        #region Constants

        public const string FBXHeaderExtension = "FBXHeaderExtension";
        public const string Document = "Document";
        public const string References = "References";
        public const string Definitions = "Definitions";
        public const string Objects = "Objects";
        public const string Relations = "Relations";
        public const string Connections = "Connections";
        public const string ObjectData = "ObjectData";
        public const string Takes = "Takes";
        public const string Version5 = "Version5";
        public const string ObjectType = "ObjectType";
        public const string Model = "Model";
        public const string Material = "Material";
        public const string Texture = "Texture";
        public const string Video = "Video";
        public const string SceneInfo = "SceneInfo";
        public const string GlobalSettings = "GlobalSettings";
        public const string Deformer = "Deformer";        
        public const string SubDeformer = "SubDeformer";
        public const string Pose = "Pose";
        public const string Properties60 = "Properties60";
        public const string Property = "Property";
        public const string Vertices = "Vertices";
        public const string PolygonVertexIndex = "PolygonVertexIndex";
        public const string LayerElementNormal = "LayerElementNormal";
        public const string Normals = "Normals";
        public const string LayerElementUV = "LayerElementUV";
        public const string UV = "UV";
        #endregion

        FBXBindingManager bindingManager;

        public FBXDocument()
        {
            Declarations = new SortedList<string, List<FBXDeclarationNode>>();
        }

        public SortedList<string, List<FBXDeclarationNode>> Declarations { get; set; }


        public FBXBindingManager BindingManager { get { return bindingManager; } }

        public void Add(FBXDeclarationNode node)
        {
            List<FBXDeclarationNode> list;
            if (!Declarations.TryGetValue(node.Type, out list))
            {
                list = new List<FBXDeclarationNode> { node };
                Declarations.Add(node.Type, list);
            }
            else
                list.Add(node);
        }

        public T GetDeclaration<T>(string type,int index=0) where T : FBXDeclarationNode
        {
            List<FBXDeclarationNode> values;
            if (Declarations.TryGetValue(type, out values) && index < values.Count)
                return (T)values[index];
            return null;
        }

        public int DeclarationCount(string type)
        {
            List<FBXDeclarationNode> values;
            if (Declarations.TryGetValue(type, out values))
                return values.Count;
            return 0;
        }

        public FBXObject GetObject(string type, int index = 0)
        {
            return GetDeclaration<FBXObject>(type, index);
        }

        public static FBXDocument Load(string FBXfilename)
        {
            ANTLRFileStream fStream = new ANTLRFileStream(FBXfilename, Encoding.ASCII);
            fbxLexer lexer = new fbxLexer(fStream);
            fbxParser parser = new fbxParser(new CommonTokenStream(lexer));

            FBXDocument doc = parser.document();
            doc.bindingManager = new FBXBindingManager(doc);
            return doc;
        }
    }

    public abstract class FBXDeclarationNode
    {
        public string Type { get; set; }

        public override string ToString()
        {
            return Type;
        }
    }
}
