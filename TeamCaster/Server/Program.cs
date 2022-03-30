using System;
using Server.Logging;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPServer server = new TCPServer("127.0.0.1", 22222, new ConsoleLogger());
            server.Start();
        }
    }
}
