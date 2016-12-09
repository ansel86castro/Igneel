using Igneel.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Importers
{
    public interface IAnimationImporter
    {
        void ImportAnimation(Scene scene, string filename);

        void ImportAnimation(Scene scene, string filename, Frame root, string fileRoot);
    }

}
