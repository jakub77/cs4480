using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            for (; ; )
            {
                Console.WriteLine("Enter URL");
                string raw = Console.ReadLine();
                string host, URI;
                int split = raw.IndexOf('/');
                if (split == -1)
                {
                    host = raw;
                    URI = string.Empty;
                }
                else
                {
                    host = raw.Substring(0, split);
                    URI = raw.Substring(split);
                }

                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                byte[] buffer = new byte[1024];

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
                    string request = "GET " + URI + " HTTP/1.0" + "\r\nHost: " + host + "\r\n\r\n";
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
                    string data = Encoding.ASCII.GetString(buffer);
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
