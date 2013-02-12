using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ProxyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ProxyServer proxy = new ProxyServer();
            proxy.Listen();
        }
    }

    class ProxyServer
    {
        int bufferSize = 1024 * 100;
        Socket listenSocket;
        public ProxyServer()
        {
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(new IPEndPoint(IPAddress.Any, 888));
        }

        public void Listen()
        {
            listenSocket.Listen(100);
            for (; ; )
            {
                Socket client = listenSocket.Accept();
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(client);
            }
        }

        public void HandleClient(object clientSock)
        {
            Socket client = (Socket)clientSock;

            string recieved = getMessage(client);
            HttpRequest request = new HttpRequest(recieved);

            string webPage = getWebPage(request);

            client.Send(System.Text.Encoding.ASCII.GetBytes(webPage));
            client.Close();
        }

        public string getMessage(Socket s)
        {
            byte[] buffer = new byte[bufferSize];
            int bytesReceived = s.Receive(buffer);
            if (bytesReceived == 0)
            {
                Console.WriteLine("Socket closed?");
                return String.Empty;
            }
            return Encoding.ASCII.GetString(buffer, 0, bytesReceived);
        }
        
        public string getWebPage(HttpRequest request)
        {
            Socket webClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                webClient.Connect(request.host, request.port);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect: " + e.Message);
                return String.Empty;
            }

            try
            {
                string req = request.ToString();
                Console.WriteLine("Request Sent was:\n---------\n" + req + "\n----------\n");
                byte[] toSend = System.Text.Encoding.ASCII.GetBytes(req);
                webClient.Send(toSend);
            }
            catch (Exception e)
            {
                Console.WriteLine("Send Error: " + e.Message);
                return string.Empty;
            }

            try
            {
                byte[] buffer = new byte[bufferSize];
                int recv = webClient.Receive(buffer);
                string data = Encoding.ASCII.GetString(buffer, 0, recv);
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine("Receive Error: " + e.Message);
                return string.Empty;
            }
        }


    }
}
/*
            Console.WriteLine("Host is: " + request.host);
            Console.WriteLine("Port is: " + request.port);
            Console.WriteLine("URI is: " + request.URI);
            Console.WriteLine("Version is: " + request.version);
 */
