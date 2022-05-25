using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TeamCaster.Core.Network.IO
{
    class TCPPacketManager
    {
        public TCPPacketManager()
        {
        }

        public TCPPacket BuildPacket<T>(T data)
        {
            return new TCPPacket(data);
        }

        public byte[] SerializePacket(TCPPacket tcpPacket)
        { 
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, tcpPacket);

            memoryStream.Close();
            return memoryStream.ToArray();
        }

        public TCPPacket DeserializePacket(byte[] tcpPacket)
        {
            MemoryStream memoryStream = new MemoryStream(tcpPacket);
            //memoryStream.Seek(0, SeekOrigin.Begin);
            memoryStream.Position = 0;

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            TCPPacket deserializedPacket = (TCPPacket)binaryFormatter.Deserialize(memoryStream);

            memoryStream.Close();

            return deserializedPacket;
        }

        public dynamic ParsePacket(TCPPacket tcpPacket)
        {
            return tcpPacket.Data;
        }
    }
}
