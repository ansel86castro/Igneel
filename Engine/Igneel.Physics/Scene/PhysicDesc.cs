using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
    public class PhysicDesc
    {
        public string Name;
        public SceneFlags Flags = SceneFlags.SeparateThread | SceneFlags.DisableSceneMutex;
        public Vector3 Gravity = new Vector3(0, -9.8f, 0);

    }
}
