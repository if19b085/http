using System;

namespace HTTP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server on port 8080!");
            HTTPServer server = new HTTPServer(8080);
            server.Start();
        }
    }
}
