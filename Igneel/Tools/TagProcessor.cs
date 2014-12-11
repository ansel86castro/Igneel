using Igneel.Components;
using Igneel.Graphics;
using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Igneel.TagProcesors
{
  
    public abstract class TagProcessor
    {
        protected static Dictionary<string, object> nodesLookup = new Dictionary<string, object>();
        static List<TagProcessor> processors = new List<TagProcessor>();

        static TagProcessor()
        {
            foreach (var item in typeof(TagProcessor).Assembly.DefinedTypes)
            {
                if(item.IsSubclassOf(typeof(TagProcessor)))
                {
                    var instance = (TagProcessor)Activator.CreateInstance(item);
                    processors.Add(instance);
                }
            }
        }

        public static void Register(TagProcessor processor)
        {
            processors.Add(processor);
        }        

        public static void Remove(TagProcessor processor)
        {
            processors.Remove(processor);
        }

        public abstract object Process(Scene scene, SceneNode node);

        protected static object GetObject(string name)
        {
            object obj = null;
            nodesLookup.TryGetValue(name, out obj);
            return obj;
        }

        public static void ProcessHeirarchy(Scene scene, SceneNode node)
        {
            _ProcessHeirarchy(scene,node);
            nodesLookup.Clear();
        }

        private static void _ProcessHeirarchy(Scene scene, SceneNode node)
        {
            if (node.Tag != null)
            {
                foreach (var proc in processors)
                {
                    object value;
                    if ((value = proc.Process(scene, node)) != null)
                    {
                        nodesLookup.Add(node.Name, value);
                        break;
                    }
                }
            }

            foreach (var child in node.Childrens.ToArray())
            {
                ProcessHeirarchy(scene,child);
            }
        }

        public static void BindTechnique(Scene scene, SceneNode node, Match match, NodeTechnique technique)
        {
            SceneNode affector = match.Groups["A"].Success ? scene.GetNode(match.Groups["A"].Value) : node;
            technique.Affector = affector;
            technique.UpdatePose(affector.GlobalPose);

            if (node.Childrens.Count > 0)
            {
                foreach (var item in node.EnumerateNodesInPreOrden())
                {
                    if (item.NodeObject is IMeshContainer)
                    {
                        item.Technique = technique;
                    }
                }
            }

            if (node.NodeObject is IMeshContainer)
            {
                node.Technique = technique;
            }
        }
    }
}
