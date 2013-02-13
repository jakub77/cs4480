using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace TestApp
{
    class Program
    {
        static int bufferSize = 1024 * 100;
        static void Main(string[] args)
        {
            for (; ; )
            {
                Console.WriteLine("Enter URL");
                string URL = Console.ReadLine();

                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                byte[] buffer = new byte[bufferSize];

                try
                {
                    server.Connect("localhost", 888);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Unable to connect: " + e.Message);
                    continue;
                }

                try
                {
                    string request = "GET " + URL + " HTTP/1.0\r\n\r\n";
                    byte [] toSend = System.Text.Encoding.ASCII.GetBytes(request);
                    server.Send(toSend);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Send Error: " + e.Message);
                    continue;
                }

                try
                {
                    int recv = server.Receive(buffer);
                    string data = Encoding.ASCII.GetString(buffer, 0, recv);
                    buffer = new byte[bufferSize];
                    Console.WriteLine(data);                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Receive Error: " + e.Message);
                    continue;
                }

            }
        }
    }
}
