using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Input
{
    public class MouseController:IDynamic
    {
        Mouse _mouse;
        ButtonState[] _buttonStates;
        float[] _pressTime;

        public event EventHandler<MouseEventArg> MouseDown;
        public event EventHandler<MouseEventArg> MouseUp;
        public event EventHandler<MouseEventArg> MousePressed;
        public event EventHandler<MouseEventArg> MouseMoved;


        public MouseController(Mouse mouse)
        {
            _mouse = mouse;
            //{ MouseButton.Left, MouseButton.Right, MouseButton.Middle };
            _buttonStates = new ButtonState[3];
            _pressTime = new float[3];
        }

        public Mouse Mouse { get { return _mouse; } }

        public ButtonState GetButtonState(MouseButton button)
        {
            if (button == MouseButton.None)
                return ButtonState.Up;

            return _buttonStates[(int)button];
        }

        public float GetPressedTime(MouseButton button)
        {
            if (button == MouseButton.None) return 0;
            return _pressTime[(int)button];
        }

        public void Update(float elapsedTime)
        {
            bool fireDown = false, fireUp = false, firePress = false;

            for (int i = 0; i < 3; i++)
            {
                if (_mouse.IsButtonPresed((MouseButton)i))
                {
                    if (_buttonStates[i] == ButtonState.Up)
                    {
                        _buttonStates[i] = ButtonState.Down;
                        _pressTime[i] = 0;
                        fireDown = true;

                    }
                    else
                    {
                        _buttonStates[i] = ButtonState.Press;
                        _pressTime[i] += elapsedTime;
                        firePress = true;
                    }
                }
                else if (_buttonStates[i] != ButtonState.Up)
                {
                    _buttonStates[i] = ButtonState.Up;
                    _pressTime[i] = 0;
                    fireUp = true;
                }
            }

            MouseEventArg arg = new MouseEventArg(this);
            if (fireDown && MouseDown != null)
            {
                MouseDown(this, arg);
            }

            if (firePress && MousePressed != null)
            {
                MousePressed(this, arg);
            }

            if (fireUp && MouseUp != null)
            {
                MouseUp(this, arg);
            }

            if ((arg.Dx != 0 || arg.Dy != 0 || arg.Dz != 0) && MouseMoved!=null)
            {
                MouseMoved(this, arg);
            }
        }
    }


}
