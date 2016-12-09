using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForgeEditor
{
    public interface ISceneView
    {
        void OnSceneChanged(Igneel.SceneManagement.Scene scene);
    }
}
