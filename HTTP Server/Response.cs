using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace HTTP_Server
{
    /*
    HTTP/1.1 200 OK
Via: HTTP/1.1 proxy_server_name
Server: Apache/1.3
Content-type: text/html, text, plain
Content-length: 78

<html>
<head>
<title>HTTP</TITLE>


</head>
<body>
<p> HTTP/1.1-Demo</p>
</body>
</html>
        */
    class Response
    {

        public void Post(NetworkStream stream, string message, string status , string contentType)
        {
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(HTTPServer.VERSION + " " + status);
            sb.AppendLine("Content-Type: " + contentType);
            sb.AppendLine("Content-Length: " + Encoding.UTF8.GetBytes(message).Length);
            sb.AppendLine();
            sb.AppendLine(message);
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            writer.Write(sb.ToString());
        }

       
    }
}
