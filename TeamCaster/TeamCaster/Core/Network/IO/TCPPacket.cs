using System;

namespace TeamCaster.Core.Network.IO
{
    [Serializable]
    class TCPPacket
    {
        private dynamic _data;
        private DateTime _sendTime;

        public dynamic Data { get => _data; }

        public DateTime SendTime { get => _sendTime; }

        public TCPPacket(dynamic data)
        {
            _data = data;
            _sendTime = DateTime.Now;
        }
    }
}
