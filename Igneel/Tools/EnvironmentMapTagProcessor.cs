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
    class EnvironmentMapTagProcessor : TagProcessor
    {
        Regex pattern = new Regex(@"__env((?<D>(d)))?(_s(?<S>\d+))?(_a(?<A>\w+))?__");

        public override object Process(Scene scene, SceneNode node)
        {
            if(scene == null)
                return null;

            string tag = node.Tag;
            if (tag == null)
                return null;
            var match = pattern.Match(tag);
            if (!match.Success) return null;

            EnvironmentMapTechnique env;
            if (match.Groups["S"].Success)
                env = new EnvironmentMapTechnique(int.Parse(match.Groups["S"].Value)) { IsDynamic = match.Groups["D"].Success };
            else
                env = new EnvironmentMapTechnique() { IsDynamic = match.Groups["D"].Success };

            BindTechnique(scene, node, match, env);          
           
            return env;
        }
    }
}
