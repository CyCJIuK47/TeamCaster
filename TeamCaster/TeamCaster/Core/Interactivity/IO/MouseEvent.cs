using System;

namespace TeamCaster.Core.Interactivity.IO
{
    [Serializable]
    class MouseEvent
    {   
        public double RelativeX { get; set; }

        public double RelativeY { get; set; }

        public MouseEventType EventType { get; set; }

        public MouseEvent(double relativeX, double relativeY, MouseEventType mouseEventType)
        {
            RelativeX = relativeX;
            RelativeY = relativeY;

            EventType = mouseEventType;
        }
    }
}
