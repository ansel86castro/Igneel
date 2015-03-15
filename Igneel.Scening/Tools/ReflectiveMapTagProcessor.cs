using Igneel.Graphics;
using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Igneel.Scenering.TagProcesors
{
    class ReflectiveMapTagProcessor : TagProcessor
    {
        Regex pattern = new Regex(@"__rf(?<R>(l|r))?(_s(?<S>\d+))?(_p(?<P>\w+))(_a(?<A>\w+))__");

        public override object Process(Scene scene, SceneNode node)
        {
            if (scene == null)
                return null;

            string tag = node.Tag;
            var match = pattern.Match(tag);
            if (!match.Success) return null;

            SceneNode planeMesh = match.Groups["P"].Success ? scene.GetNode(match.Groups["P"].Value) : node;
            if (planeMesh == null || !(planeMesh.NodeObject is IMeshContainer))
                return null;   
         
            var mesh = ((IMeshContainer)planeMesh.NodeObject).Mesh;
            Plane plane;
            if (!mesh.IsPlane(out plane))
                return null;

            float aspect = (float)GraphicDeviceFactory.Device.BackBuffer.Width/(float)GraphicDeviceFactory.Device.BackBuffer.Height;
            int width = GraphicDeviceFactory.Device.BackBuffer.Width;
             if (match.Groups["S"].Success)
                 width = int.Parse(match.Groups["S"].Value);
            int height = (int)((float)width / aspect);

            ReflectiveNodeTechnique tech = new ReflectiveNodeTechnique(width, height, plane);             
            if(match.Groups["R"].Success)
            {
                if(match.Groups["R"].Value=="l")
                {
                    tech.UseReflection = true;
                    tech.UseRefraction = false;
                }
                else
                {
                    tech.UseReflection = false;
                    tech.UseRefraction = true;
                }
            }
            BindTechnique(scene, node, match, tech);

            return tech;
        }
    }
}
