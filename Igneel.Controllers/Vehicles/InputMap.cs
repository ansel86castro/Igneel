using Igneel.Input;
using Igneel.Scenering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Controllers
{
    public interface IVehicleInputMap
    {
        bool IsActive { get; }
    }

    public class KeyboardInputMap:IVehicleInputMap
    {
        Keys mapKey;

        public KeyboardInputMap(Keys key)
        {
            mapKey = key;
        }

        public Keys MapKey { get { return mapKey; } set { mapKey = value; } }

        public bool IsActive
        {
            get { return Engine.KeyBoard.IsKeyPressed(mapKey); }
        }
    }
}
