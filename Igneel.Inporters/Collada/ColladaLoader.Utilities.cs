﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices;
using Igneel.Rendering;
using Igneel.Graphics;
using ClrRuntime;
using Igneel.Scenering;

namespace Igneel.Importers.Collada
{
    partial class ColladaImporter
    {
        Regex numericExp = new Regex(@"(-?\d+(\.\d+)?)");
        Regex wordExp = new Regex(@"\w+");

        Regex texExp = new Regex(@"(?<R>.+)(-|_)(?<N>\w+)");

        string[] tags = new string[] { "-d", "_d", "-D", "_D" };
        Func<string, string>[] tagConv = new Func<string, string>[] { x => "-" + x.ToLower(), x => "_" + x.ToLower(), x => "-" + x, x => "_" + x };

        internal ElementRef InvokeParser(XElement element, object @param)
        {
            Func<XElement, object, ElementRef> method;
            if (handlerLookup.TryGetValue(element.Name.LocalName, out method))
            {
                var refe = method(element, param);
                return refe;
            }
            return null;
        }

        internal ElementRef InvokeParser(XElement searchRoot, string attr, string attrValue, object @param)
        {
            var item = searchRoot.Descendants().Where(x => x.GetAttribute(attr) == attrValue).First();
            return InvokeParser(item, param);
        }

        internal ElementRef ResolveElement(XElement searchRoot, string attr, string attrValue, object @param)
        {            
            if (references.ContainsKey(attrValue))
                return references[attrValue];
            else
            {
                var item = searchRoot.Descendants().Where(x => x.GetAttribute(attr) == attrValue).First();

                var refe = InvokeParser(item, param);
                if (refe != null && !references.ContainsKey(attrValue))                
                    references.Add(attrValue, refe);                               
                return refe;
            }
        }

        internal ElementRef ResolveElementById(XElement root, string id, object @param = null)
        {
            if (references.ContainsKey(id))
                return references[id];
            else
            {
                var item = root.GetDescendantById(id);
                if (item != null)
                {
                    Func<XElement, object, ElementRef> method;
                    if (handlerLookup.TryGetValue(item.Name.LocalName, out method))
                    {
                        var refe = method(item, param);
                        if (refe != null && !references.ContainsKey(id))
                        {
                            references.Add(id, refe);
                            return refe;
                        }
                    }
                }
                return null;
            }
        }

        internal ElementRef ResolveElementByAttr(XElement root, string attr, string value, object @param = null)
        {
            if (references.ContainsKey(value))
                return references[value];
            else
            {
                var item = root.GetDescendantByAttr(attr, value);
                if (item != null)
                {
                    Func<XElement, object, ElementRef> method;
                    if (handlerLookup.TryGetValue(item.Name.LocalName, out method))
                    {
                        var refe = method(item, param);
                        if (refe != null && !references.ContainsKey(value))
                        {
                            references.Add(value, refe);
                            return refe;
                        }
                    }
                }
                return null;
            }
        }

        internal ElementRef ResolveElement(XElement element, object @param = null)
        {
            string id = element.GetId();
            if (id != null && references.ContainsKey(id))
                return references[id];
            else
            {
                Func<XElement, object, ElementRef> method;
                if (handlerLookup.TryGetValue(element.Name.LocalName, out method))
                {
                    var refe = method(element, param);
                    if (refe != null && id != null && !references.ContainsKey(id))
                        references.Add(id, refe);
                    return refe;
                }
                return null;
            }
        }

        internal ElementRef ResolveElementById(string id, object @param = null)
        {
            return ResolveElementById(rootElement, id, param);
        }

        internal ElementRef ResolveElementByUrl(XElement root, string url, object @param = null)
        {
            return ResolveElementById(root, url.Substring(1), param);
        }

        internal ElementRef ResolveElementByTag(XElement root, string tag, object @param = null)
        {
            var element = root.GetDescendantByTag(tag);
            return ResolveElement(element, param);
        }

        internal T ResolveElement<T>(XElement element, object @param)
        {
            Func<XElement, object, ElementRef> method;
            if (handlerLookup.TryGetValue(element.Name.LocalName, out method))
            {
                var refe = method(element, param);
                if (refe != null)
                    return (T)refe.Object;
            }
            return default(T);
        }

        internal void ReadColorInfo(XElement element, string tag, Action<Vector4, string> callback)
        {
            Vector4 color = new Vector4(1);
            string textureFilename = null;

            var e = element.GetDescendantByTag(tag);
            if (e != null)
            {
                var colorElement = e.GetDescendantByTag("color");
                if (colorElement != null)
                    color = ParseVector4(colorElement.Value, false);

                var textureRefElement = e.GetDescendantByTag("texture");
                if (textureRefElement != null)
                {
                    var reference = textureRefElement.GetAttribute("texture");
                    var source = library_images.GetElementById(reference);
                    if (source != null)
                    {
                        textureFilename = GetFilenameFromUrl(source.GetDescendantByTag("init_from").Value);
                    }
                    else
                    {
                        var sourceParameter = (EffectParameter)ResolveElement(element, "sid", reference, null).Object;
                        if (sourceParameter.Value is Sampler2D && ((Sampler2D)sourceParameter.Value).Texture != null)
                            textureFilename = ((Sampler2D)sourceParameter.Value).Texture.ToString();
                    }
                }
                try
                {
                    if (textureFilename != null)
                    {
                        Uri uri = new Uri(textureFilename);
                        textureFilename = uri.LocalPath;
                        if (!File.Exists(textureFilename)) 
                            textureFilename = null;
                    }
                }
                catch (UriFormatException) { }

                callback(color, textureFilename);
            }
        }

        internal string GetImageFilenameFromUrl(string id)
        {
            var xImage = library_images.GetElementById(id);
            string url = xImage.GetElementByTag("init_from").Value;
            return GetFilenameFromUrl(url);
        }

        internal string GetFilenameFromUrl(string url)
        {
            Uri uri;
            string filename;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                filename = Path.Combine(directory, url.Replace("%20", " "));
                if (!File.Exists(filename))
                    return null;//throw new FileNotFoundException(filename);
                return filename;
            }
            else
            {
                filename = uri.LocalPath;
                if (!File.Exists(filename))
                {
                    filename = Path.Combine(directory, Path.GetFileName(filename));
                }
            }

            if (filename == null)
                throw new UriFormatException();

            return filename;
        }

        internal static void ReadFloatInfo(XElement element, string tag, Action<float> callback)
        {
            var e = element.GetDescendantByTag(tag);
             if (e != null)
             {
                 float value;
                 var floatElement = e.GetDescendantByTag("float");
                 if (floatElement != null)
                 {
                     value = float.Parse(floatElement.Value);
                     callback(value);
                 }
             }
        }

        internal void ReadParamenterSurfaceInfo(XElement element, string sid, Action<string> callback)
        {
            foreach (var item in element.Descendants("newparam"))
            {
                if (item.GetAttribute("sid") == sid)
                {
                    var surfaceElement = item.GetElementByTag("surface");
                    if (surfaceElement != null)
                    {
                        var initFrom = surfaceElement.GetElementByTag("init_from");
                        if (initFrom != null)
                        {
                            callback(initFrom.Value);
                            return;
                        }
                    }
                }
            }
        }

        internal void ReadTextureInfo(XElement element, string tag, Action<string> callback)
        {
            var e = element.GetDescendantByTag(tag);
            if (e != null)
            {              
                var textureRefElement = e.GetDescendantByTag("texture");
                if (textureRefElement != null)
                {
                    var textureElement = library_images.GetElementById(textureRefElement.GetAttribute("texture"));
                    var textureUrl = textureElement.GetDescendantByTag("init_from").Value;
                    callback(textureUrl);
                }               
            }
        }

        internal Vector4 ParseVector4(string text, bool invert = true)
        {
            var values = numericExp.Matches(text);

            if (z_up && invert)
            {
                return new Vector4(float.Parse(values[0].Value),
                   float.Parse(values[2].Value),
                   float.Parse(values[1].Value),
                   float.Parse(values[3].Value));
            }
            else
            {
                return new Vector4(float.Parse(values[0].Value),
                    float.Parse(values[1].Value),
                    float.Parse(values[2].Value),
                    float.Parse(values[3].Value));
            }
        }

        internal Vector3 ParseVector3(string text, bool invert = true)
        {
            var m = numericExp.Matches(text);   

            if (z_up && invert)
            {
                return new Vector3(float.Parse(m[0].Value),
                   float.Parse(m[2].Value),
                   float.Parse(m[1].Value));
            }
            else return new Vector3(float.Parse(m[0].Value),
                   float.Parse(m[1].Value),
                   float.Parse(m[2].Value));
        }

        internal Vector2 ParseVector2(string text)
        {
            var m = numericExp.Matches(text);    
            return new Vector2(float.Parse(m[0].Value), float.Parse(m[1].Value));
        }

        internal float[] ParseFloatArray(string text)
        {
            var m =  numericExp.Matches(text);                             
            float[] floatValues = new float[m.Count];

            for (int i = 0; i < floatValues.Length; i++)
            {
                floatValues[i] = float.Parse(m[i].Value);
            }

            return floatValues;
        }

        internal int[] ParseIntArray(string text)
        {
            var m = numericExp.Matches(text);
            int[] floatValues = new int[m.Count];

            for (int i = 0; i < floatValues.Length; i++)
            {
                floatValues[i] = int.Parse(m[i].Value);
            }

            return floatValues;
        }

        internal bool[] ParseBoolArray(string text)
        {
            string[] values = text.Split(' ');
            bool[] floatValues = new bool[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                floatValues[i] = bool.Parse(values[i]);
            }

            return floatValues;
        }

        internal string[] ParseNameArray(string text)
        {
            var m = wordExp.Matches(text);

            string[] values = new string[m.Count];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = m[i].Value;
            }

            return values;
        }

        internal unsafe Matrix ParseMatrix(string text)
        {
            float[] array = ParseFloatArray(text);
            Matrix mat = new Matrix();

            fixed (float* pArray = array)
            {            
                Runtime.Copy(pArray, &mat, 16 * 4);
            }
          
            return ConvertMatrix(mat);
        }

        //protected void ParseTransforms(Object3D node, XElement element)
        //{
        //    //Collada Transform = Scale * Rotation * Translation

        //    var translate = element.GetElementByTag("translate");
        //    var scaling = element.GetElementByTag("scale");
        //    Matrix rotMat = Matrix.Identity;

        //    if (translate != null)
        //        node.LocalTraslation = (ParseVector3(translate.Value) - node.Pivot);
        //    if (scaling != null)
        //        node.LocalScale = ParseVector3(scaling.Value);
          
        //    foreach (var item in element.GetElementsByTag("rotate"))
        //    {
        //        Vector4 v = ParseVector4(item.Value);
        //        rotMat = Matrix.RotationAxis(v.ToVector3(), -Numerics.ToRadians(v.W)) * rotMat;
        //    }
                   

        //    node.EulerAngles = Euler.FromMatrix(rotMat);
        //}

        internal ColladaType ParsePrimitiveType(string p)
        {
            switch (p)
            {
                case "float":
                    return ColladaType.Float;
                case "int":
                    return ColladaType.Int;
                case "bool":
                    return ColladaType.Bool;
                case "name":
                    return ColladaType.Name;                
            }
            return ColladaType.Unknown;
        }

        internal SceneNode[] GetMeshBones(Source source)
        {
            string[] boneNames = (string[])source.Array;
            SceneNode[] bones = new SceneNode[boneNames.Length];

            for (int i = 0; i < bones.Length; i++)
                bones[i] = (SceneNode)ResolveElementByAttr(library_visual_scenes, "name" , boneNames[i], null).Object;
            return bones;
        }

        protected Matrix ConvertMatrix(Matrix mat)
        {
            mat = Matrix.Transpose(mat);

            if (z_up)
            {
                //Vector4 column1 = mat.GetColumn(1);
                //Vector4 column2 = mat.GetColumn(2);
                //mat.SetColumn(1, column2);
                //mat.SetColumn(2, column1);


                //Vector4 row1 = mat.GetRow(1);
                //Vector4 row2 = mat.GetRow(2);
                //mat.SetRow(1, row2);
                //mat.SetRow(2, row1);

                var x = Swap(mat.Right);
                var y = Swap( mat.Up);
                var z = Swap(mat.Front);
                var t = Swap(mat.Translation);
                mat = new Matrix(x, z, y, t);
            }
            return mat;
        }

        private Vector3 Swap(Vector3 v)
        {
            return new Vector3(v.X, v.Z, v.Y);
        }

        protected Vector3 ConvertVector3(Vector3 v , bool isAxis = true)
        {
            if (z_up && isAxis)
            {
                float z = v.Z;
                v.Z = v.Y;
                v.Y = z;
            }
            return v;
        }

        protected Vector4 ConvertVector4(Vector4 v, bool isAxis = true)
        {
            if (z_up && isAxis)
            {
                float z = v.Z;
                v.Z = v.Y;
                v.Y = z;
            }
            return v;
        }

        protected Quaternion ConvertQuaternion(Quaternion v)
        {
            if (z_up)
            {
                float z = v.Z;
                v.Z = v.Y;
                v.Y = z;
            }
            v.W = -Numerics.ToRadians(v.W);
            return v;
        }

        protected Vector4 ConvertQuaternion(Vector4 v)
        {
            if (z_up)
            {
                float z = v.Z;
                v.Z = v.Y;
                v.Y = z;
            }
            v.W = -Numerics.ToRadians(v.W);
            return v;
        }

        internal unsafe Matrix[] ConvertToMatrices(Source source)
        {
            Matrix[] matrices = new Matrix[source.Accesor.Count];
            source.Lock();

            fixed (Matrix* pMatrices = matrices)
            {
                Runtime.Copy((void*)source.ArrayPointer, pMatrices, source.Array.Length * 4);
            }

            for (int i = 0; i < matrices.Length; i++)            
                matrices[i] = ConvertMatrix(matrices[i]);                            

            source.Unlock();
            return matrices;
        }

        internal unsafe void ConvertIntoVector3(Source source)
        {           
            source.Lock();

            Vector3* pter = (Vector3*)source.ArrayPointer;
            for (int i = 0; i < source.Accesor.Count; i++, pter++)            
                *pter = ConvertVector3(*pter);            

            source.Unlock();
        }

        internal unsafe void ConvertIntoVector4(Source source)
        {
            source.Lock();

            Vector4* pter = (Vector4*)source.ArrayPointer;
            for (int i = 0; i < source.Accesor.Count; i++, pter++)
                *pter = ConvertVector4(*pter);

            source.Unlock();
        }

        internal unsafe void ConvertIntoMatrix(Source source)
        {
            source.Lock();

            Matrix* pter = (Matrix*)source.ArrayPointer;
            for (int i = 0; i < source.Accesor.Count; i++, pter++)
                *pter = ConvertMatrix(*pter);

            source.Unlock();
        }

        internal unsafe void ConvertIntoQuaternion(Source source)
        {
            source.Lock();

            Vector4* pter = (Vector4*)source.ArrayPointer;
            for (int i = 0; i < source.Accesor.Count; i++, pter++)
                *pter = ConvertQuaternion(*pter);

            source.Unlock();
        }

        internal unsafe void ConvertInto<T>(Source source, Func<T, T> converter)
            where T:struct
        {
            source.Lock();
            
            float* pter = (float*)source.ArrayPointer;
            int stride = source.Accesor.Stride;

            for (int i = 0; i < source.Accesor.Count; i++, pter += stride)
            {
                T data =converter(ClrRuntime.Runtime.GetValue<T>(pter));
                ClrRuntime.Runtime.SetValue(data , pter);
            }

            source.Unlock();
        }

        internal void ConvertInto(Source source)
        {
            if (source.Type != ColladaType.Float) return;

            if (source.Accesor.Stride == 3)
                ConvertIntoVector3(source);
            else if (source.Accesor.Stride == 4)
                ConvertIntoQuaternion(source);
            else if (source.Accesor.Stride == 16)
                ConvertIntoMatrix(source);
        }

        internal static T[] CheckForContantValue<T>(T[] array) where T : struct
        {            
            return array.Length > 0 && Array.TrueForAll(array, x => x.Equals(array[0])) ?
                new T[1] { array[0] } : 
                array;
        }

        public static bool Equals(float[] x, float[] y)
        {
            if (x.Length != y.Length) return false;

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                    return false;
            }

            return true;
        }

        public string GetTexture(string filename, string type)
        {
            string file;
            for (int i = 0; i < tags.Length; i++ )
            {
                string tag = tags[i];
                var converter = tagConv[i];
                int index = filename.IndexOf(tag);
                if (index >= 0)
                {
                    file = filename.Replace(tag, converter(type));
                    if (File.Exists(file))
                        return file;
                }
            }
            return null;                
        }

        public void SetMaps(MeshMaterial material)
        {
            var diffuseFilename = material.DiffuseMap.ToString();                  
            var diffuseName = Path.GetFileNameWithoutExtension(diffuseFilename);
            Match match = texExp.Match(diffuseName);
            if (!match.Success)
                return;

            diffuseName = match.Groups["R"].Value;
            string value = match.Groups["N"].Value;
            if (value[0] != 'D' && value[0] != 'd')
                return;

            string mapsDirectory = Path.GetDirectoryName(diffuseFilename);   
            foreach (var file in Directory.EnumerateFiles(mapsDirectory))
            {              
                match = texExp.Match(Path.GetFileNameWithoutExtension(file));
                if (match.Success)
                {
                    var name = match.Groups["R"].Value;
                    if (name == diffuseName)
                    {
                        value = match.Groups["N"].Value;
                        if (value[0] == 'n')
                            material.NormalMap = GraphicDeviceFactory.Device.CreateTexture2DFromFile( file );
                        else if (value[0] == 's')
                            material.SpecularMap =GraphicDeviceFactory.Device.CreateTexture2DFromFile( file );
                    }
                }
            }
        }
    }
}