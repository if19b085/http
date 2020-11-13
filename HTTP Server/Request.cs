using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace HTTP_Server
{
    class Request
    {
        public string Method { get; } 
        public string Command { get; }
        public string Identifier { get; }
        public string Version { get; }
        public string ContentType { get; }
        public string ContentLength { get; }
        public string Payload { get; }

        /*Später Split mit Substrings verbessern*/
        /*String gehören noch getrimmt
         z.B. "GET " zu "GET"*/
        public Request(string _request)
        {
            string[] lines = _request.Split("\r\n");
            string[] firstline = lines[0].Split("/");
            Method = firstline[0];
            Command = firstline[1];
            Version = firstline[3];


            string[] identifier = firstline[2].Split(" ");
            Debug.WriteLine("*" + identifier[0] + "*");

            if (!isCorrectIdentifier(identifier[0]))
            {
                Identifier = "all";
            }
            else
            {
                Identifier = identifier[0];
            }
            /*
              int i = 1;
              foreach (var line in lines)
              {
                  string[] pairs = line.Split(":");
                  Console.WriteLine(line + ": " + i);
                  i++;
              }
            */
            foreach (var line in lines)
            {
                string[] pairs = line.Split(":");
                if (String.Compare(pairs[0], "Content-Type") == 0)
                {
                    ContentType = pairs[1];
                }
                if (String.Compare(pairs[0], "Content-Length") == 0)
                {
                    ContentLength = pairs[1];
                }
            }

            Payload = GetRequestMessage(_request);
        }

        //Das geht irgendwie anders, keine Ahnung. bin c++ gwohnt
        public string GetMethod()
        {
            return Method;
        }
        public string GetMethodFromRequest(string _request)
        {
            if (string.IsNullOrEmpty(_request))
            {
                return null;
            }

            string method = "";
            string[] tokens = _request.Split("/");
            method = tokens[0];
            return method;
        }

        private static string GetRequestMessage(string request)
        {
            if (string.IsNullOrEmpty(request))
            {
                return null;
            }
            string[] tokens = request.Split("\r\n\r\n");
            string message = tokens[1];
            return message;
        }

        public string GetLogEntry()
        {

            string logEntry = Method + " /" + Command + "/" + Identifier;
            return logEntry;
        }

        private bool isCorrectIdentifier(string identifier)
        {
            if(string.IsNullOrEmpty(identifier))
            {
                return false;
            }
            for (int i = 0; i < identifier.Length; i++)
            {
                if (Char.IsNumber(identifier[i]))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}