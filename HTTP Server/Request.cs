using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace HTTP_Server
{
    class Request
    {
        public string Method;
        public string Command;
        public string Identifier;
        public string Version;
        public string ContentType;
        public string ContentLength;
        public string Payload;
        
        /*Später Split mit Substrings verbessern*/
        public Request(string _request)
        {
            string[] lines = _request.Split("\r\n");
            string[] firstline = lines[0].Split("/");
            Method = firstline[0];
            Command = firstline[1];
            Version = firstline[3];
            string[] identifier = firstline[2].Split(" ");
            Identifier = identifier[0];
            /*
              int i = 1;
              foreach (var line in lines)
              {
                  string[] pairs = line.Split(":");
                  Console.WriteLine(line + ": " + i);
                  i++;
              }
            */
              foreach(var line in lines)
              {
                  string[] pairs = line.Split(":");
                  if(String.Compare(pairs[0] , "Content-Type") == 0)
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
            /* Entries be like:
            lists all messages: GET /messages
            add message: POST /messages 
            show first message: GET /messages/1
            show third message: GET /messages/3
            update first message: PUT /messages/1 (Payload: the message)
            remove first message: DELETE /messages/1
            */
            string logEntry = Method + " /" + Command +  "/" + Identifier;
            return logEntry;
        }
    }
}
