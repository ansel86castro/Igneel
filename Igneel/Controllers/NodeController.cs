using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Igneel.Collections;
using System.Reflection;
using Igneel.Assets;
using System.Text.RegularExpressions;
using Igneel.Components;

namespace Igneel.Controllers
{   

    public class ControllerAttribute : Attribute
    {
        public Type MatchType { get; set; }
        public string MatchPattern { get; set; }
    }

    [Serializable]
    public abstract class NodeController : ResourceAllocator, IDynamic, INodeController
    {
        private SceneNode node;

        [Browsable(false)]
        [AssetMember(storeAs: StoreType.Reference)]
        public SceneNode Node 
        { 
            get { return node; }
            set { node = value; }
        }

        public virtual void Initialize(SceneNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            this.node = node;
            this.node.IsDynamic = true;
        }

        protected override void OnDispose(bool d)
        {
            if (d)
            {
                if (node != null)
                    node.IsDynamic = false;
            }
            base.OnDispose(d);
        }

        public abstract void Update(float elapsedTime);

        public Asset CreateAsset()
        {
            return Asset.Create(this);
        }
    }

    //public static class NodeControllerManager
    //{
    //    //public static List<NodeController> controllers = new List<NodeController>();

    //    //public static List<NodeController> Controllers { get { return controllers; } }

    //    public static void LinkControllers(Assembly assembly)
    //    {
    //        List<Type> findControllers = new List<Type>();
    //        List<ControllerAttribute> attrsController = new List<ControllerAttribute>();
    //        var types = assembly.GetExportedTypes();
    //        foreach (var type in types)
    //        {
    //            ControllerAttribute[] attrs = (ControllerAttribute[])type.GetCustomAttributes(typeof(ControllerAttribute), true);
    //            if (attrs.Length > 0)
    //            {
    //                findControllers.Add(type);
    //                attrsController.Add(attrs[0]);
    //            }
    //        }

    //        foreach (var item in Engine.SceneManager.Scenes)
    //        {
    //            LinkControllerToNode(findControllers, attrsController, item.Root);
    //        }

    //    }

    //    private static void LinkControllerToNode(List<Type> controllers, List<ControllerAttribute> attrs, SceneNode rootNode)
    //    {
    //        //foreach (var item in rootNode.GetDescendantsInPosOrden())
    //        //{
    //        //    if (item.Controller != null)
    //        //    {
    //        //        item.Controller.Initialize(item);
    //        //    }
    //        //    else
    //        //    {
    //        //        for (int i = 0; i < attrs.Count; i++)
    //        //        {
    //        //            if (attrs[i].MatchType == item.GetType() ||
    //        //                (attrs[i].MatchPattern != null && Regex.IsMatch(item.Name, attrs[i].MatchPattern)))
    //        //            {
    //        //                var controller = (NodeController)Activator.CreateInstance(controllers[i]);
    //        //                item.Controller = controller;

    //        //                controller.Initialize(item);
    //        //            }
    //        //        }
    //        //    }
    //        //}
    //    }
    //}

}
