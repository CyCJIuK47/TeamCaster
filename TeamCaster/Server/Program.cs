using System;
using Server.Logging;

namespace Server
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Write("IP: ");
            string ip = Console.ReadLine();

            Console.Write("Port: ");
            int port = Int32.Parse(Console.ReadLine());
            
            Console.WriteLine();

            try
            {
                TCPServer server = new TCPServer(ip, port, ServerType.Resend, new ConsoleLogger());
                server.Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

    }
}
