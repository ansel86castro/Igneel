using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Input
{
    public class KeyBoardController:IDynamic
    {
        private Keyboard _kb;
        private ButtonState[] _buttonStates;
        private float[] _pressTime;

        public event EventHandler<KeyEventArg> KeyDown;
        public event EventHandler<KeyEventArg> KeyUp;
        public event EventHandler<KeyEventArg> KeyPressed;       

        public KeyBoardController(Keyboard keyboard)
        {
            this._kb = keyboard;
            _buttonStates = new ButtonState[256];
            _pressTime = new float[256];
        }

        public ButtonState GetKeyState(Keys key)
        {
            return _buttonStates[(int)key];
        }

        public float GetPressedTime(Keys key)
        {
            return _pressTime[(int)key];
        }

        public void Update(float elapsedTime)
        {
            bool fireDown = false, fireUp = false, firePress = false;

            for (int i = 1; i < 256; i++)
            {              
                if (_kb.IsKeyPressed((Keys)i))
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

            KeyEventArg arg = new KeyEventArg(this);
            if (fireDown && KeyDown != null)
            {
                KeyDown(this, arg);
            }

            if (firePress && KeyPressed != null)
            {
                KeyPressed(this, arg);
            }

            if (fireUp && KeyUp != null)
            {
                KeyUp(this, arg);
            }           
        }
    }
}
