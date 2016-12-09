using Igneel.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel;

namespace ForgeEditor
{
    public class SceneViewModel
    {
        ISceneView view;
        Scene scene;
       
        public SceneViewModel(ISceneView view)
        {
            this.view = view;
            scene = Engine.Scene;
        }

        public Scene Scene
        {
            get { return scene; }
            set {
                scene = value;
                view.OnSceneChanged(scene);
            }
        }
    }
}
