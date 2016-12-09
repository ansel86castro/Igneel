using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Input
{
    public struct KeyEventArg
    {
        private KeyBoardController keyBoardController;

        public KeyEventArg(KeyBoardController keyBoardController)
        {
            // TODO: Complete member initialization
            this.keyBoardController = keyBoardController;
        }

        public ButtonState GetState(Keys key)
        {
            return keyBoardController.GetKeyState(key);
        }

        public bool IsDown(Keys key)
        {
            return GetState(key) == ButtonState.Down;
        }

        public bool IsPressed(Keys key)
        {
            return GetState(key) == ButtonState.Press;
        }

        public bool IsUp(Keys key)
        {
            return GetState(key) == ButtonState.Up;
        }

        public float GetPressedTime(Keys key)
        {
            return keyBoardController.GetPressedTime(key);
        }
    }
}
