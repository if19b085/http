using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HTTP_Server
{
    public class HTTPServer
    {
        public const String MSG_DIR = "/root/msg/";
        public const String WEB_DIR = "/root/web";
        public const String VERSION = "HTTP/1.1";
        public const String NAME = "MCTG";
        private bool running = false;

        private TcpListener listener;

        public HTTPServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        private void Run()
        {
            running = true;
            listener.Start();

            while(running)
            {
                Console.WriteLine("Waiting for connection..");

                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine("Client connected!");

                HandleClient(client);

                client.Close();
            }

            running = false;
            listener.Stop();
        }

        private void HandleClient(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream());

            String msg = "";
            while(reader.Peek() != -1)
            {
                msg += (char) reader.Read();
            }

            Console.WriteLine("Request: \n" + msg);

            Request request = new Request(msg);
            Console.WriteLine("Method:" + request.Method);
            Console.WriteLine("Indentifier:" + request.Identifier);
            Console.WriteLine("Command:" + request.Command);
            Console.WriteLine("Version:" + request.Version);
            Console.WriteLine("ContentType:" + request.ContentType);
            Console.WriteLine("ContentLength:" + request.ContentLength);
            Console.WriteLine("Payload:" + request.Payload);
        }
    }
}

