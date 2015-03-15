using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Igneel.Input
{
    public delegate void AxisAction(int nCount);

    public enum JoystickAxis { X = 0, Y = 1, Z = 2, RZ = 4 }
    public enum MouseAxis { X = 0, Y = 1, Wheel = 2 }
    public enum MouseButton 
    { 
        None = -1, 
        Left =  0, 
        Right = 1, 
        Middle = 2
    }

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
            public ButtonMapping onTransition;
            public ButtonMapping mapping;
        }

        const int AXIS_RANGE = 1000;
    
        Dictionary<int, MappingStage> keyboardMap;
        Dictionary<int, MappingStage> mouseButtonMap;
        Dictionary<int, MappingStage>[] joystickButtonMap;
        Dictionary<MouseAxis, MouseAxisMapping> axisMouseMap;
        Dictionary<JoystickAxis, JoystickAxisMapping>[] axisJoystickMap;
      
        public InputManager()
        {            
            keyboardMap = new Dictionary<int, MappingStage>();
            mouseButtonMap = new Dictionary<int, MappingStage>();                
            axisMouseMap = new Dictionary<MouseAxis, MouseAxisMapping>();      

            Service.Set<InputManager>(this);
        }

       

        private void MapStage(Dictionary<int, MappingStage> map, int key, Action action, bool bTransition)
        {
            if (map.ContainsKey(key))
            {
                if (bTransition)
                    map[key].onTransition = new ButtonMapping() { Keys = (int)key, Action = action };
                else
                    map[key].mapping = new ButtonMapping() { Keys = (int)key, Action = action };
            }
            MappingStage m = new MappingStage();
            if (bTransition)
                m.onTransition = new ButtonMapping() { Keys = (int)key, Action = action };
            else
                m.mapping = new ButtonMapping() { Keys = (int)key, Action = action };
            map.Add((int)key, m);
        }

        public void MapKeyboardAction(Keys key, Action proc, bool bTransition)
        {
            MapStage(keyboardMap, (int)key, proc, bTransition);
        }

        public void MapMouseButtonAction(MouseButton nButton, Action proc, bool bTransition)
        {
            MapStage(mouseButtonMap, (int)nButton, proc, bTransition);
        }

        public void MapJoystickButtonAction(int joystick, int nButton, Action proc, bool bTransition)
        {
            if (joystickButtonMap[joystick] == null)
            {
                joystickButtonMap[joystick] = new Dictionary<int, MappingStage>();
            }
            MapStage(joystickButtonMap[joystick], nButton, proc, bTransition);
        }

        public void MapMouseAxisAction(MouseAxis axis, AxisAction proc)
        {
            if (axisMouseMap.ContainsKey(axis))
            {
                var map = axisMouseMap[axis];
                map.Action += proc;
            }
            else
                axisMouseMap.Add(axis, new MouseAxisMapping { Axis = axis, Action = proc });
        }

        public void MapJoystickAxisAction(int joystick, JoystickAxis axis, AxisAction proc)
        {
            if (axisJoystickMap[joystick] == null)
            {
                axisJoystickMap[joystick] = new Dictionary<JoystickAxis, JoystickAxisMapping>();
            }
            if (axisJoystickMap[joystick].ContainsKey(axis))
            {
                var map = axisJoystickMap[joystick][axis];
                map.Action += proc;
            }
            else
                axisJoystickMap[joystick].Add(axis, new JoystickAxisMapping { Axis = axis, Action = proc });
        }

        public void UnMapKeyboardAction(Keys key)
        {
            keyboardMap.Remove((int)key);
        }

        public void UnMapMouseButtonAction(MouseButton button)
        {
            mouseButtonMap.Remove((int)button);
        }

        public void UnMapJoystickButtonAction(int joy, int nButton)
        {
            joystickButtonMap[joy].Remove(nButton);
        }

        public void UnMapMouseAxisAction(MouseAxis axis)
        {
            axisMouseMap.Remove(axis);
        }

        public void UnMapJoystickAxisAction(int joy, JoystickAxis axis)
        {
            axisJoystickMap[joy].Remove(axis);
        }

        public void ClearActionMaps()
        {
            keyboardMap.Clear();
            mouseButtonMap.Clear();            
            axisMouseMap.Clear();
            if (joystickButtonMap != null)
            {
                for (int i = 0; i < joystickButtonMap.Length; i++)
                {
                    joystickButtonMap[i].Clear();
                    axisJoystickMap[i].Clear();
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
