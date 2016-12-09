using System.Text.RegularExpressions;
using Igneel.SceneManagement;

namespace Igneel.Utilities
{

    /// <summary>
    /// __a[d][k][m5.5][b_nodeName]__  : create actor from scene node
    ///
    ///d: create dynamic actor
    ///k: kept node in scene else remove it
    ///b: node binding
    ///nodeName :name of the node to bind
    ///
    ///
    ///Notes:
    ///for create a wheell shape, you need to create a cylindre and rotate it -90 degrees arount Z-axis( Y-axis in 3DMAX)
    /// </summary>
    class ActorTagProcessor:TagProcessor
    {
        Regex _nodeMetaRegexAlt = new Regex(@"__a((?<TYPE>(d)))?(?<KEEP>k)?(m((?<X>\d+)((\.|_)(?<Y>\d+))?))?(b_(?<BINDING>\w+))?__");        

        public ActorTagProcessor()
        {

        }

        public override object Process(Scene scene, Frame node)
        {
            if (scene == null || scene.Physics == null)
                return null;
            
            string tag = node.Tag;
            if (tag == null) return null;
     
            var match = _nodeMetaRegexAlt.Match(tag);

            if (!match.Success) return null;

            var isDynamic = match.Groups["TYPE"].Success;
            var bindGroup = match.Groups["BINDING"];

            Frame affectable = null;
            if (bindGroup.Success)
                affectable = scene.FindNode(bindGroup.Value);

            var dispose = !match.Groups["KEEP"].Success;         

            float density = 1;
            float mass = 0;
            if (match.Groups["X"].Success)
            {
                density = 0;
                mass = float.Parse(match.Groups["X"].Value + "." + match.Groups["Y"].Value);
            }
            var actor = node.CreateActor(scene.Physics, isDynamic, density , mass );
            actor.Name = node.Name;

            if (dispose)
            {
                node.Remove();
                node.Dispose();
            }

            if (affectable != null && affectable != node)
                affectable.BindTo(actor);
          
            return actor;
        }
    }
}
