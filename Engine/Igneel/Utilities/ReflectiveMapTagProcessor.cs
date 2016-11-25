using System.Text.RegularExpressions;
using Igneel.Graphics;
using Igneel.SceneComponents;
using Igneel.SceneManagement;
using Igneel.Techniques;

namespace Igneel.Utilities
{
    class ReflectiveMapTagProcessor : TagProcessor
    {
        Regex _pattern = new Regex(@"__rf(?<R>(l|r))?(_s(?<S>\d+))?(_p(?<P>\w+))(_a(?<A>\w+))__");

        public override object Process(Scene scene, Frame node)
        {
            if (scene == null)
                return null;

            string tag = node.Tag;
            var match = _pattern.Match(tag);
            if (!match.Success) return null;

            Frame planeMesh = match.Groups["P"].Success ? scene.FindNode(match.Groups["P"].Value) : node;
            if (planeMesh == null || !(planeMesh.Component is IFrameMesh))
                return null;   
         
            var mesh = ((IFrameMesh)planeMesh.Component).Mesh;
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
