﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class TCPServer
    {
        private IPEndPoint _ipPoint;
        private Socket _listenSocket;
        private List<Socket> _clientPool;

        private ILogger _logger;

        public string IP
        {
            get { return _ipPoint.Address.ToString(); }
        }

        public int Port
        {
            get { return _ipPoint.Port; }
        }

        public TCPServer(string IP, int port, ILogger logger = null)
        {
            _ipPoint = new IPEndPoint(IPAddress.Parse(IP), port);
            _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientPool = new List<Socket>();

            _logger = logger;
        }

        public void Start()
        {
            try
            {
                _listenSocket.Bind(_ipPoint);
                _listenSocket.Listen(10);

                _logger?.Log("Server", "Server is up.");

                while (true)
                {
                    Socket newClient = _listenSocket.Accept();
                    _clientPool.Add(newClient);

                    Thread onNewClientThread = new Thread(() => _onNewClientHandler(newClient));
                    onNewClientThread.Start();

                    _logger?.Log("Server|NewConnection", $"{newClient.RemoteEndPoint}");
                }
            }
            catch (Exception ex)
            {
                _logger?.Log("Server|NewConnection", $"{ex.Message}");
            }
        }

        public void Send(Socket target, byte[] data)
        {
            target.Send(BitConverter.GetBytes(data.Length));
            target.Send(data);
        }

        public byte[] Receive(Socket target)
        {
            byte[] msg_size_bytes = new byte[4];
            target.Receive(msg_size_bytes);

            int msg_size = BitConverter.ToInt32(msg_size_bytes, 0);

            byte[] msg = new byte[msg_size];
            target.Receive(msg);

            return msg;
        }

        public void Shutdown()
        {
            _listenSocket.Shutdown(SocketShutdown.Both);
            _listenSocket.Close();

            _logger?.Log("Server", "Server is down.");
        }

        private void _onNewClientHandler(Socket newClient)
        {
            while (true)
            {
                byte[] data = this.Receive(newClient);

                _logger?.Log("Server", $"Received data from {newClient.RemoteEndPoint}");

                this.Send(newClient, data);
            }
        }
    }
}
