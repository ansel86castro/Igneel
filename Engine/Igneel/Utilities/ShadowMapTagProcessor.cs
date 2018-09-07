using System;
using System.Text.RegularExpressions;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
using Igneel.States;
using Igneel.Techniques;

namespace Igneel.Utilities
{
    class ShadowMapTagProcessor : TagProcessor
    {
        Regex _pattern = new Regex(@"__sm((?<D>(d)))?(_s(?<S>\d+))?(_n(?<A>\w+))(?<K>_k)?__");

        public override object Process(Scene scene, Frame node)
        {
            if (scene == null)
                return null;

            string tag = node.Tag;

             var match = _pattern.Match(tag);
             if (!match.Success) return null;
            
             ShadowMapTechnique sm;
             if (match.Groups["S"].Success)             
                 sm = new ShadowMapTechnique(int.Parse(match.Groups["S"].Value)) { IsDynamic = match.Groups["D"].Success };  
             else
                 sm = new ShadowMapTechnique() { IsDynamic = match.Groups["D"].Success };

             Camera camera = (Camera)node.Component;
             Frame affector = scene.FindNode(match.Groups["A"].Value);                     
             sm.Camera = camera;

             sm.Affector = affector;             
             sm.UpdatePose(affector.GlobalPose);
             affector.Technique = sm;

             if (!match.Groups["K"].Success)
             {
                 node.Component = null;
                 node.Remove();
                 node.Dispose();
             }

             if (!EngineState.Shadow.ShadowMapping.Enable)
             {
                 EngineState.Shadow.ShadowMapping.Enable = true;
                 EngineState.Shadow.Enable = true;
             }

             return sm;
        }
    }

    public class StaticShadowMapTagProcessor : TagProcessor
    {
        Regex _pattern = new Regex(@"__ssm(_s(?<S>\d+))?(_l(?<L>\w+))__");

        public override object Process(Scene scene, Frame node)
        {
            string tag = node.Tag;

            var match = _pattern.Match(tag);
            if (!match.Success) return null;

            StaticShadowMapTechnique sm;
            if (match.Groups["S"].Success)
                sm = new StaticShadowMapTechnique(int.Parse(match.Groups["S"].Value));
            else
                sm = new StaticShadowMapTechnique();
           
            sm.Camera = (Camera)node.Component;            
            sm.Affector = node;

            Frame group = scene.FindNode(match.Groups["L"].Value);
            foreach (var item in group.EnumerateNodesInPreOrden())
            {
                if (item.Component !=null)
                {
                    item.Technique = sm;
                }
            }

            if (!match.Groups["K"].Success)
            {
                node.Component = null;
                node.Dispose();
            }
            return sm;
        }
    }
}
