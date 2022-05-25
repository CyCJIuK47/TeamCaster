using System;
using TeamCaster.Core.Network;
using TeamCaster.Core.Interactivity.IO;

namespace TeamCaster.Core.Interactivity
{
    class ActionRecorder
    {
        private INetworkDataSender _networkDataSender;

        public ActionRecorder(INetworkDataSender networkDataSender)
        {
            _networkDataSender = networkDataSender;
        }

        public void RecordEvent(double relativePosX, double relativePosY, MouseEventType mouseEventType)
        {
            _networkDataSender.Send(new MouseEvent(relativePosX, relativePosY, mouseEventType));
        }

        public void RecordEvent(MouseEvent mouseEvent)
        {
            _networkDataSender.Send(mouseEvent);
        }

    }
}
