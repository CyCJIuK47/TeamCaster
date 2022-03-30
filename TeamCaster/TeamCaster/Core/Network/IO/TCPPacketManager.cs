using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TeamCaster.Core.Network.IO
{
    class TCPPacketManager
    {
        private BinaryFormatter _binaryFormatter;

        public TCPPacketManager()
        {
            _binaryFormatter = new BinaryFormatter();
        }

        public TCPPacket BuildPacket<T>(T data)
        {
            return new TCPPacket(data);
        }

        public byte[] SerializePacket(TCPPacket tcpPacket)
        {
            MemoryStream memoryStream = new MemoryStream();
            _binaryFormatter.Serialize(memoryStream, tcpPacket);

            return memoryStream.ToArray();
        }

        public TCPPacket DeserializePacket(byte[] tcpPacket)
        {
            MemoryStream memoryStream = new MemoryStream(tcpPacket);
            TCPPacket deserializedPacket = (TCPPacket)_binaryFormatter.Deserialize(memoryStream);

            return deserializedPacket;
        }

        public dynamic ParsePacket(TCPPacket tcpPacket)
        {
            return tcpPacket.Data;
        }
    }
}
