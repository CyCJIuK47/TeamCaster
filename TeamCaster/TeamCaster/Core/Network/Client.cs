using System;
using System.Net;
using System.Net.Sockets;
using TeamCaster.Core.Network.IO;

namespace TeamCaster.Core.Network
{
    class TCPClient : INetworkDataSender, INetworkDataReceiver
    {
        private IPEndPoint _ipPoint;
        private Socket _hostSocket;
        private TCPPacketManager _TCPPacketManager;

        public string IP
        {
            get { return _ipPoint.Address.ToString(); }
        }
        public int Port
        {
            get { return _ipPoint.Port; }
        }

        public TCPClient(string IP, int port)
        {
            _ipPoint = new IPEndPoint(IPAddress.Parse(IP), port);
            _hostSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
           
            _hostSocket.SendBufferSize = 65536;
            _hostSocket.ReceiveBufferSize = 65536;
            _hostSocket.NoDelay = true;
            
            _TCPPacketManager = new TCPPacketManager();
        }

        public void Connect()
        {
            _hostSocket.Connect(_ipPoint);
        }

        public void Send<T>(T data)
        {
            TCPPacket dataPacket = _TCPPacketManager.BuildPacket(data);
            byte[] byte_data = _TCPPacketManager.SerializePacket(dataPacket);

            _hostSocket.Send(BitConverter.GetBytes(byte_data.Length));
            _hostSocket.Send(byte_data);
        }

        public dynamic Receive()
        {
            byte[] msg_size_bytes = new byte[4];
            _hostSocket.Receive(msg_size_bytes);

            int msg_size = BitConverter.ToInt32(msg_size_bytes, 0);

            byte[] msg = new byte[msg_size];
            _hostSocket.Receive(msg);

            TCPPacket tcpPacket = _TCPPacketManager.DeserializePacket(msg);
            return _TCPPacketManager.ParsePacket(tcpPacket);
        }

        public void Shutdown()
        {
            _hostSocket.Shutdown(SocketShutdown.Both);
            _hostSocket.Close();
        }

    }
}
