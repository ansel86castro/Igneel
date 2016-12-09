using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using System.IO;
using System.Threading;
using Igneel.Assets;

namespace Igneel.Importers
{
    [ImportFormat(".obj")]
    public class OBJImporter : AssetImporter
    {
        protected override EngineContent Import(string filename)
        {
            ModelLoaderOBJ loader = new ModelLoaderOBJ(filename);
            var objModel = loader.Read();
            var result =  objModel.ToSceneNode();

            return result;
        }   
    }

    class ModelLoaderOBJ
    {
        #region CONSTANTS

        const string GROUP = "g";
        const string OBJECT = "o";
        const string FACE = "f";
        const string POSITION = "v";
        const string NORMAL = "vn";
        const string TEXTCOORD = "vt";
        const string USE_MATERIAL = "usemtl";
        const string MATERIALS = "mtllib";
        const string COMMENTS = "#";
        const string NEW_MATERIAL = "newmtl";
        const string AMBIENT = "Ka";
        const string DIFUSE = "Kd";
        const string SPECULAR = "Ks";
        const string TRASPARENCY = "d";
        const string REFLECTIVITY = "rfl";
        const string REFRACTIVITY = "rfr";
        const string SPECULAR_POWER = "Ns";
        const string ILIMINATION_MODE = "illum"; //1-specular enable  2-specular disable
        const string TEXTURE_DIFUSE_FILE = "map_Kd";
        const string TEXTURE_NORMAL_FILE = "map_bump";

        char[] delimeters = { ' ', '\t' };
        char[] faceDelimeter = { '/' };

        #endregion

        List<Vector3> positions;
        List<Vector3> normals;
        List<Vector2> textCoord;

        StreamReader reader;
        OBJModel model;      
        Thread thread;
        string modelName;
        FileInfo file;

        public ModelLoaderOBJ(string filename)
        {
            file = new FileInfo(filename);
            modelName = file.Name;
            reader = file.OpenText();        
            positions = new List<Vector3>();
            normals = new List<Vector3>();
            textCoord = new List<Vector2>();
            thread = new Thread(new ParameterizedThreadStart(ReadMaterials));
        }

        //public ModelLoaderOBJ(Stream stream)
        //{
        //    reader =    new StreamReader(stream);
        //    modelName = "model";
        //    positions = new List<Vector3>();
        //    normals = new List<Vector3>();
        //    textCoord = new List<Vector2>();
        //    thread =    new Thread(new ParameterizedThreadStart(ReadMaterials));
            
        //}

        public OBJModel Model { get { return model; } }

        public OBJModel Read()
        {
             string curDirectory = Environment.CurrentDirectory;
             Environment.CurrentDirectory = file.DirectoryName;
            try
            {               
                Group currentGroup = null;
                Layer currentLayer = null;
                string currentMat = null;               
                model = new OBJModel() { Name = modelName };      

                string line = reader.ReadLine();                                       

                while (line != null)
                {
                    if (!line.StartsWith(COMMENTS) && !string.IsNullOrEmpty(line))
                    {
                        string[] comand = line.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);

                        //Si la linea comienza con g es una definición de grupo, crear un nuevo grupo
                        if (comand[0] == GROUP)
                        {
                            currentGroup = comand.Length > 1 ? model.CreateGroup(line.Substring(comand[0].Length).Trim()) :
                                model.CreateGroup(null);

                            currentGroup.StartVertex = int.MaxValue;;
                           
                            currentLayer = null;

                            if (currentMat == null) currentMat = "default";
                        }
                        else if (comand[0] == MATERIALS)
                        {                           
                            thread.Start(comand[1]);
                        }

                        else
                        {                          
                            if (comand[0] == USE_MATERIAL)
                            {
                                if (currentGroup == null)                                
                                    currentGroup = model.CreateGroup(null);

                                currentMat = comand[1];
                                currentLayer = currentGroup.CreateLayer(currentMat);
                            }
                            else if (comand[0] == POSITION)
                            {
                                model.SetFormat(OBJModel.VertexFormat.Position);                              
                                positions.Add(GetVector3(comand));
                            }
                            else if (comand[0] == TEXTCOORD)
                            {
                                model.SetFormat(OBJModel.VertexFormat.TexCoord);
                                textCoord.Add(GetVector2(comand));
                            }
                            else if (comand[0] == NORMAL)
                            {
                                model.SetFormat(OBJModel.VertexFormat.Normal);
                                normals.Add(GetVector3(comand));
                            }
                            else if (comand[0] == FACE)
                            {
                                if (currentGroup == null)
                                {
                                    currentGroup = model.CreateGroup(null);
                                    currentGroup.StartVertex = int.MaxValue;
                                    if (currentMat == null) currentMat = "default";
                                }

                                if (currentLayer == null)
                                    currentLayer = currentGroup.CreateLayer(currentMat);

                                //si las caras son triangulos 
                                if (comand.Length == 4)
                                {
                                    Face face = GetFace(comand);
                                    currentLayer.Faces.Add(face);
                                    currentGroup.FaceCount++;
                                    currentGroup.StartVertex = Math.Min(currentGroup.StartVertex, 
                                        Math.Min(Math.Min(face.V0.Position, face.V1.Position), face.V2.Position));
                                }
                                //las caras son un traingle strip ,obtener todos los triangulos del strip
                                else
                                {
                                    var faceList = GetFaceStrip(comand);                                  
                                    for (int i = 0; i < faceList.Count; i++)
                                    {
                                        Face face = faceList[i];
                                        currentGroup.StartVertex = Math.Min(currentGroup.StartVertex,
                                        Math.Min(Math.Min(face.V0.Position, face.V1.Position), face.V2.Position));
                                    }

                                    currentLayer.Faces.AddRange(faceList);
                                    currentGroup.FaceCount += faceList.Count;
                                }
                            }
                        }

                    }
                    line = reader.ReadLine();
                }

                if (thread.ThreadState == ThreadState.Running)
                    thread.Join();

                model.Positions = positions.ToArray();
                if (normals.Count > 0)
                    model.Normals = normals.ToArray();
                if (textCoord.Count > 0)
                    model.TexCoords = textCoord.ToArray();

                Environment.CurrentDirectory = curDirectory;
                return model;
            }                            
            finally
            {
                reader.Close();
                Environment.CurrentDirectory = curDirectory;
            }
            
        }      

        //void AddVertex(ObjModel model, FaceIndex[] face)
        //{
        //    for (int i = 0; i < 3; i++)
        //        model.AddVertex(face[i].PositionIndex);
        //}

        private void ReadMaterials(object @param)
        {
            string filename = (string)@param;          
            Material currentMaterial=null;
            List<string> textures = new List<string>();         
            using (StreamReader matReader = new StreamReader(filename,Encoding.Default))
            {
                string line = matReader.ReadLine();
                while (line != null)
                {
                    if (!line.StartsWith(COMMENTS) && !String.IsNullOrEmpty(line))
                    {
                        if (line.StartsWith(NEW_MATERIAL))
                        {
                            if (currentMaterial != null)
                            {
                                currentMaterial.Textures = textures.ToArray();                                
                            }

                            textures.Clear();
                            currentMaterial = new Material(line.Substring(NEW_MATERIAL.Length).Trim());
                            model.AddMaterial(currentMaterial);
                            currentMaterial.Transparency = 1;
                        }
                        else
                        {
                            string[] commnd = line.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                            if (commnd.Length > 0)
                            {
                                if (commnd[0] == AMBIENT)
                                    currentMaterial.Ambient = GetVector3(commnd);

                                else if (commnd[0] == DIFUSE)
                                    currentMaterial.Difusse = GetVector3(commnd);

                                else if (commnd[0] == SPECULAR)
                                    currentMaterial.Specular = GetVector3(commnd);

                                else if (commnd[0] == SPECULAR_POWER)
                                    currentMaterial.SpecularPower = GetValue(commnd);

                                else if (commnd[0] == TRASPARENCY)
                                    currentMaterial.Transparency = GetValue(commnd);
                                else if (commnd[0] == REFLECTIVITY)
                                    currentMaterial.Reflectivity = GetValue(commnd);
                                else if (commnd[0] == REFRACTIVITY)
                                    currentMaterial.Refractivity = GetValue(commnd);
                                else if (commnd[0] == TEXTURE_DIFUSE_FILE)
                                    textures.Add(GetTextureFileName(file.DirectoryName, line));
                                else if (commnd[0] == TEXTURE_NORMAL_FILE)
                                    currentMaterial.NormalMap = GetTextureFileName(file.DirectoryName, line);
                            }   
                        }
                    }
                    line = matReader.ReadLine();
                }
                if (currentMaterial != null)
                {
                    currentMaterial.Textures = textures.ToArray();
                }
            }         
        }

        private string GetTextureFileName(string directoryName, string line)
        {
            string file = directoryName + "\\";

            int baseIndex = 0;  
        
            //shift while is white space
            while (baseIndex < line.Length && char.IsWhiteSpace(line[baseIndex]))
                baseIndex++;

            //shift while not is white space
            while (baseIndex < line.Length && !char.IsWhiteSpace(line[baseIndex]))
                baseIndex++;

            //shift while is white space
            while (baseIndex < line.Length && char.IsWhiteSpace(line[baseIndex]))
                baseIndex++;
                         
            file = line.Substring(baseIndex, line.Length - baseIndex).Trim();
            FileInfo fi = new FileInfo(file);

            if(!fi.Exists)
                return directoryName + "\\" + file;

            return fi.FullName;
        }

        private Vector3 GetVector3(string[] values)
        {
            Vector3 v = new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
            return v;
        }
       
        private Vector2 GetVector2(string[] values)
        {
            Vector2 v = new Vector2(float.Parse(values[1]), float.Parse(values[2]));
            return v;
        }

        private Face GetFace(string[] values)
        {
            Face triangle = new Face(vertexCount:3);                      
            
            triangle.Vertexes[0] = new Vertex(DecodeIndexValues(values[1]));
            triangle.Vertexes[1] = new Vertex(DecodeIndexValues(values[2]));
            triangle.Vertexes[2] = new Vertex(DecodeIndexValues(values[3]));

            return triangle;
        }

        private int[] DecodeIndexValues(string values)
        {
            string[] indexes = values.Split(faceDelimeter);

            int[] indValues = new int[indexes.Length];

            for (int k = 0; k < indexes.Length; k++)
            {
                if (String.IsNullOrEmpty(indexes[k]))
                    indValues[k] = -1;
                else
                    indValues[k] = int.Parse(indexes[k]) - 1;
            }
            return indValues;
        }
        
        private List<Face> GetFaceStrip(string[] values)
        {
            List<Face> faces = new List<Face>();          

            Face traingle = new Face(vertexCount: 3);            
            traingle.Vertexes[0] = new Vertex(DecodeIndexValues(values[1]));
            traingle.Vertexes[1] = new Vertex(DecodeIndexValues(values[2]));
            traingle.Vertexes[2] = new Vertex(DecodeIndexValues(values[3]));
            faces.Add(traingle);

            for (int i = 4; i < values.Length; i++)
            {
                traingle = new Face(vertexCount:3);               
                traingle.Vertexes[0] = new Vertex(DecodeIndexValues(values[i]));
                traingle.Vertexes[1] = new Vertex(DecodeIndexValues(values[i - 1]));
                traingle.Vertexes[2] = new Vertex(DecodeIndexValues(values[i - 2]));
            }
            return faces;
        }

        private float GetValue(string[] values)
        {
            return float.Parse(values[1]);
        }               

    }

  

}
