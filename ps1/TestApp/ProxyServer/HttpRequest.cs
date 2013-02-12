using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyServer
{
    class HttpRequest
    {
        string CRLF = "\r\n";
        int HTTP_PORT = 80;
        public string method { get; set; }
        public string URI { get; set; }
        public string version { get; set; }
        public string headers { get; set; }
        public string host { get; set; }
        public int port { get; set; }

        public override string ToString()
        {
            String request;
            request = method + " " + URI + " " + version + CRLF;
            request += headers;
            request += "Connection: close" + CRLF;
            request += CRLF;
            return request;
        }
        

        public HttpRequest(string request)
        {
            headers = String.Empty;
            try
            {
                if (request.Length == 0)
                {
                    Console.WriteLine("Request is empty");
                    return;
                }
                string[] requestLines = request.Split(CRLF.ToCharArray());
                string[] tmp = requestLines[0].Split(' ');
                method = tmp[0];
                URI = tmp[1];
                version = tmp[2];
                Console.WriteLine("URI is: " + URI);
                if (!method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                    Console.WriteLine("Method is not a GET!");
                for (int i = 1; i < requestLines.Length; i++)
                {
                    if (requestLines[i].Length == 0)
                        continue;

                    headers += requestLines[i] + CRLF;

                    if(requestLines[i].StartsWith("Host:", StringComparison.OrdinalIgnoreCase))
                    {
                        tmp = requestLines[i].Split(' ');
                        if(tmp[1].IndexOf(':') > 0)
                        {
                            String[] tmp2 = tmp[1].Split(':');
                            host = tmp2[0];
                            port=int.Parse(tmp2[1]);
                        }
                        else
                        {
                            host = tmp[1];
                            port = HTTP_PORT;
                        }
                    }
                }
                if (URI.Length == 0)
                    URI = "/";
                Console.WriteLine("Host to contact is: " + host + " at port " + port);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception HttpRequest: " + e.ToString());
            }
        }
    }
}
