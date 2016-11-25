using Igneel.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel;

namespace Igneel.Controllers
{
    public interface IVehicleInputMap
    {
        bool IsActive { get; }
    }

    public class KeyboardInputMap:IVehicleInputMap
    {
        Keys _mapKey;

        public KeyboardInputMap(Keys key)
        {
            _mapKey = key;
        }

        public Keys MapKey { get { return _mapKey; } set { _mapKey = value; } }

        public bool IsActive
        {
            get { return Engine.KeyBoard.IsKeyPressed(_mapKey); }
        }
    }
}
