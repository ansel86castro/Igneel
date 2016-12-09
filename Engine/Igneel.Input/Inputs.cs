using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Igneel.Input
{
    public delegate void AxisAction(int nCount);

    public enum JoystickAxis { X = 0, Y = 1, Z = 2, Rz = 4 }
    public enum MouseAxis { X = 0, Y = 1, Wheel = 2 }

   

    struct ButtonMapping
    {
        public int Keys;
        public Action Action;
    }

    struct MouseAxisMapping
    {
        public MouseAxis Axis;
        public AxisAction Action;
    }
    struct JoystickAxisMapping
    {
        public int Joystick;
        public JoystickAxis Axis;
        public AxisAction Action;
    }   

    public abstract class InputManager: ResourceAllocator
    {
        class MappingStage
        {
            public ButtonMapping OnTransition;
            public ButtonMapping Mapping;
        }

        const int AxisRange = 1000;
    
        Dictionary<int, MappingStage> _keyboardMap;
        Dictionary<int, MappingStage> _mouseButtonMap;
        Dictionary<int, MappingStage>[] _joystickButtonMap;
        Dictionary<MouseAxis, MouseAxisMapping> _axisMouseMap;
        Dictionary<JoystickAxis, JoystickAxisMapping>[] _axisJoystickMap;
      
        public InputManager()
        {            
            _keyboardMap = new Dictionary<int, MappingStage>();
            _mouseButtonMap = new Dictionary<int, MappingStage>();                
            _axisMouseMap = new Dictionary<MouseAxis, MouseAxisMapping>();      

            Service.Set<InputManager>(this);
        }

       

        private void MapStage(Dictionary<int, MappingStage> map, int key, Action action, bool bTransition)
        {
            if (map.ContainsKey(key))
            {
                if (bTransition)
                    map[key].OnTransition = new ButtonMapping() { Keys = (int)key, Action = action };
                else
                    map[key].Mapping = new ButtonMapping() { Keys = (int)key, Action = action };
            }
            MappingStage m = new MappingStage();
            if (bTransition)
                m.OnTransition = new ButtonMapping() { Keys = (int)key, Action = action };
            else
                m.Mapping = new ButtonMapping() { Keys = (int)key, Action = action };
            map.Add((int)key, m);
        }

        public void MapKeyboardAction(Keys key, Action proc, bool bTransition)
        {
            MapStage(_keyboardMap, (int)key, proc, bTransition);
        }

        public void MapMouseButtonAction(MouseButton nButton, Action proc, bool bTransition)
        {
            MapStage(_mouseButtonMap, (int)nButton, proc, bTransition);
        }

        public void MapJoystickButtonAction(int joystick, int nButton, Action proc, bool bTransition)
        {
            if (_joystickButtonMap[joystick] == null)
            {
                _joystickButtonMap[joystick] = new Dictionary<int, MappingStage>();
            }
            MapStage(_joystickButtonMap[joystick], nButton, proc, bTransition);
        }

        public void MapMouseAxisAction(MouseAxis axis, AxisAction proc)
        {
            if (_axisMouseMap.ContainsKey(axis))
            {
                var map = _axisMouseMap[axis];
                map.Action += proc;
            }
            else
                _axisMouseMap.Add(axis, new MouseAxisMapping { Axis = axis, Action = proc });
        }

        public void MapJoystickAxisAction(int joystick, JoystickAxis axis, AxisAction proc)
        {
            if (_axisJoystickMap[joystick] == null)
            {
                _axisJoystickMap[joystick] = new Dictionary<JoystickAxis, JoystickAxisMapping>();
            }
            if (_axisJoystickMap[joystick].ContainsKey(axis))
            {
                var map = _axisJoystickMap[joystick][axis];
                map.Action += proc;
            }
            else
                _axisJoystickMap[joystick].Add(axis, new JoystickAxisMapping { Axis = axis, Action = proc });
        }

        public void UnMapKeyboardAction(Keys key)
        {
            _keyboardMap.Remove((int)key);
        }

        public void UnMapMouseButtonAction(MouseButton button)
        {
            _mouseButtonMap.Remove((int)button);
        }

        public void UnMapJoystickButtonAction(int joy, int nButton)
        {
            _joystickButtonMap[joy].Remove(nButton);
        }

        public void UnMapMouseAxisAction(MouseAxis axis)
        {
            _axisMouseMap.Remove(axis);
        }

        public void UnMapJoystickAxisAction(int joy, JoystickAxis axis)
        {
            _axisJoystickMap[joy].Remove(axis);
        }

        public void ClearActionMaps()
        {
            _keyboardMap.Clear();
            _mouseButtonMap.Clear();            
            _axisMouseMap.Clear();
            if (_joystickButtonMap != null)
            {
                for (int i = 0; i < _joystickButtonMap.Length; i++)
                {
                    _joystickButtonMap[i].Clear();
                    _axisJoystickMap[i].Clear();
                }
            }
        }

        public abstract Keyboard CreateKeyboard(IInputContext context);

        public abstract Mouse CreateMouse(IInputContext context);

        public abstract Joystick[] CreateJoysticks(IInputContext context);

        public abstract bool CheckInputStates();

        public abstract void ResetInputStates();
    }
}
