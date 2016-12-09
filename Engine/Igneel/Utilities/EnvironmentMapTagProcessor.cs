using System.Text.RegularExpressions;
using Igneel.SceneManagement;
using Igneel.Techniques;

namespace Igneel.Utilities
{
    class EnvironmentMapTagProcessor : TagProcessor
    {
        Regex _pattern = new Regex(@"__env((?<D>(d)))?(_s(?<S>\d+))?(_a(?<A>\w+))?__");

        public override object Process(Scene scene, Frame node)
        {
            if(scene == null)
                return null;

            string tag = node.Tag;
            if (tag == null)
                return null;
            var match = _pattern.Match(tag);
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
