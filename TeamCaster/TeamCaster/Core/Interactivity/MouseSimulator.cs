using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TeamCaster.Core.Interactivity.IO;

namespace TeamCaster.Core.Interactivity
{
    class MouseSimulator
    {
        private int _width;
        private int _height;

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, IntPtr dwExtraInfo);

        public MouseSimulator()
        {
            _width = Screen.PrimaryScreen.Bounds.Width;
            _height = Screen.PrimaryScreen.Bounds.Height;
        }

        public void SimulateEvent(MouseEvent mouseEvent)
        {
            SetCursorPosition(GetActualWidth(mouseEvent.RelativeX), GetActualHeight(mouseEvent.RelativeY));

            switch(mouseEvent.EventType)
            {
                case MouseEventType.LeftButtonClick:
                    LeftClick();
                    break;
                case MouseEventType.RightButtonClick:
                    RightClick();
                    break;
                default:
                    break;
            }
        }

        public void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public void LeftPress()
        {
            MouseEvent(MouseInput.LeftDown);
        }

        public void LeftRelease()
        {
            MouseEvent(MouseInput.LeftUp);
        }

        public void LeftClick()
        {
            LeftPress();
            LeftRelease();
        }

        public void LeftDoubleClick()
        {
            LeftClick();
            LeftClick();
        }

        public void RightPress()
        {
            MouseEvent(MouseInput.RightDown);
        }

        public void RightRelease()
        {
            MouseEvent(MouseInput.RightUp);
        }

        public void RightClick()
        {
            RightPress();
            RightRelease();
        }

        public void RightDoubleClick()
        {
            RightClick();
            RightClick();
        }

        private int GetActualWidth(double relativeX)
        {
            return Convert.ToInt32(relativeX * _width);
        }

        private int GetActualHeight(double relativeY)
        {
            return Convert.ToInt32(relativeY * _height);
        }

        private void MouseEvent(MouseInput mouseInput)
        {
            mouse_event((uint)mouseInput, 0, 0, 0, IntPtr.Zero);
        }

        private enum MouseInput : uint
        {
            Move = 0x01,
            LeftDown = 0x02,
            LeftUp = 0x04,
            RightDown = 0x08,
            RightUp = 0x10
        }

    }
}
