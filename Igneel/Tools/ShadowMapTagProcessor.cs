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
    class ShadowMapTagProcessor : TagProcessor
    {
        Regex pattern = new Regex(@"__sm((?<D>(d)))?(_s(?<S>\d+))?(_n(?<A>\w+))(?<K>_k)?__");

        public override object Process(Scene scene, SceneNode node)
        {
            if (scene == null)
                return null;

            string tag = node.Tag;

             var match = pattern.Match(tag);
             if (!match.Success) return null;
            
             ShadowMapTechnique sm;
             if (match.Groups["S"].Success)             
                 sm = new ShadowMapTechnique(int.Parse(match.Groups["S"].Value)) { IsDynamic = match.Groups["D"].Success };             
             else
                 sm = new ShadowMapTechnique() { IsDynamic = match.Groups["D"].Success };

             Camera camera = (Camera)node.NodeObject;
             SceneNode affector = scene.GetNode(match.Groups["A"].Value);                     
             sm.Camera = camera;

             sm.Affector = affector;             
             sm.UpdatePose(affector.GlobalPose);
             affector.Technique = sm;

             if (!match.Groups["K"].Success)
             {
                 node.NodeObject = null;
                 node.Remove();
                 node.Dispose();
             }

             if (!Engine.Shadow.ShadowMapping.Enable)
             {
                 Engine.Shadow.ShadowMapping.Enable = true;
                 Engine.Shadow.Enable = true;
             }

             return sm;
        }
    }

    public class StaticShadowMapTagProcessor : TagProcessor
    {
        Regex pattern = new Regex(@"__ssm(_s(?<S>\d+))?(_l(?<L>\w+))__");

        public override object Process(Scene scene, SceneNode node)
        {
            string tag = node.Tag;

            var match = pattern.Match(tag);
            if (!match.Success) return null;

            StaticShadowMapTechnique sm;
            if (match.Groups["S"].Success)
                sm = new StaticShadowMapTechnique(int.Parse(match.Groups["S"].Value));
            else
                sm = new StaticShadowMapTechnique();
           
            sm.Camera = (Camera)node.NodeObject;            
            sm.Affector = node;

            SceneNode group = scene.GetNode(match.Groups["L"].Value);
            foreach (var item in group.EnumerateNodesInPreOrden())
            {
                if (item.NodeObject !=null)
                {
                    item.Technique = sm;
                }
            }

            if (!match.Groups["K"].Success)
            {
                node.NodeObject = null;
                node.Dispose();
            }
            return sm;
        }
    }
}
