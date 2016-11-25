using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Igneel.Input
{
    public struct MouseEventArg
    {              
        private MouseController controller;

        public int Dx;
        public int Dy;
        public int Dz;

        public MouseEventArg(MouseController controller)
        {
            this.controller = controller;
            Dx = controller.Mouse.X;
            Dy = controller.Mouse.Y;
            Dz = controller.Mouse.Z;
        }

        public ButtonState GetState(MouseButton button)
        {
            return controller.GetButtonState(button);
        }

        public bool IsDown(MouseButton button)
        {
            return GetState(button) == ButtonState.Down;
        }

        public bool IsPressed(MouseButton button)
        {
            return GetState(button) == ButtonState.Press;
        }

        public bool IsUp(MouseButton button)
        {
            return GetState(button) == ButtonState.Up;
        }

        public float GetPressedTime(MouseButton button)
        {
            return controller.GetPressedTime(button);
        }
    }
}
