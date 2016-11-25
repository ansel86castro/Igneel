using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Igneel.Rendering;
using Igneel.SceneComponents;
using Igneel.SceneManagement;

namespace Igneel.Utilities
{
  
    public abstract class TagProcessor
    {
        protected static Dictionary<string, object> NodesLookup = new Dictionary<string, object>();
        static List<TagProcessor> _processors = new List<TagProcessor>();

        static TagProcessor()
        {
            foreach (var item in typeof(TagProcessor).Assembly.DefinedTypes)
            {
                if(item.IsSubclassOf(typeof(TagProcessor)))
                {
                    var instance = (TagProcessor)Activator.CreateInstance(item);
                    _processors.Add(instance);
                }
            }
        }

        public static void Register(TagProcessor processor)
        {
            _processors.Add(processor);
        }        

        public static void Remove(TagProcessor processor)
        {
            _processors.Remove(processor);
        }

        public abstract object Process(Scene scene, Frame node);

        protected static object GetObject(string name)
        {
            object obj = null;
            NodesLookup.TryGetValue(name, out obj);
            return obj;
        }

        public static void ProcessHeirarchy(Scene scene, Frame node)
        {
            _ProcessHeirarchy(scene,node);
            NodesLookup.Clear();
        }

        private static void _ProcessHeirarchy(Scene scene, Frame node)
        {
            if (node.Tag != null)
            {
                foreach (var proc in _processors)
                {
                    object value;
                    if ((value = proc.Process(scene, node)) != null)
                    {
                        NodesLookup.Add(node.Name, value);
                        break;
                    }
                }
            }

            foreach (var child in node.Childrens.ToArray())
            {
                ProcessHeirarchy(scene,child);
            }
        }

        public static void BindTechnique(Scene scene, Frame node, Match match, FrameTechnique technique)
        {
            Frame affector = match.Groups["A"].Success ? scene.FindNode(match.Groups["A"].Value) : node;
            technique.Affector = affector;
            technique.UpdatePose(affector.GlobalPose);

            if (node.Childrens.Count > 0)
            {
                foreach (var item in node.EnumerateNodesInPreOrden())
                {
                    if (item.Component is IFrameMesh)
                    {
                        item.Technique = technique;
                    }
                }
            }

            if (node.Component is IFrameMesh)
            {
                node.Technique = technique;
            }
        }
    }
}
