﻿using System;
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
                //Eine Connection etablieren und diese halten sntatt immer wieder eine request-based erstellen
                Debug.WriteLine("Waiting for connection..");

                TcpClient client = listener.AcceptTcpClient();

                Debug.WriteLine("Client connected!");

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

            Debug.WriteLine("Request: \n" + msg);

            Request request = new Request(msg);
            Debug.WriteLine("Method:" + request.Method);
            Debug.WriteLine("Indentifier:" + request.Identifier);
            Debug.WriteLine("Command:" + request.Command);
            Debug.WriteLine("Version:" + request.Version);
            Debug.WriteLine("ContentType:" + request.ContentType);
            Debug.WriteLine("ContentLength:" + request.ContentLength);
            Debug.WriteLine("Payload:" + request.Payload);

            if(String.Compare(request.GetMethod(), "GET ") == 0)
            {
                if(String.Compare(request.Identifier, "all") == 0)
                {
                    msg = "show all messages";
                }
                else
                {
                    msg = "show message on position" + request.Identifier;
                }
            }
            else if (String.Compare(request.GetMethod(), "POST ") == 0)
            {
                msg = "new message added";
            }
            else if (String.Compare(request.GetMethod(), "PUT ") == 0)
            { 
                if (String.Compare(request.Identifier, "all") == 0)
                {
                    msg = "message identifier not found";
                }
                else
                {
                    msg = "put new message on position " + request.Identifier;
                }
            }
            else if (String.Compare(request.GetMethod(), "DELETE ") == 0)
            {
                if (String.Compare(request.Identifier, "all") == 0)
                {
                    msg = "message identifier not found";
                }
                else
                {
                    msg = "message deleted on position " + request.Identifier;
                }
            }
            Console.WriteLine(msg + " " + request.GetLogEntry());
        }
    }
}

