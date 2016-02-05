using System;
using Microsoft.Owin.Hosting;
using SimpleApi;

namespace SimpleApiConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            const string uri = "http://localhost:12345";
            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine(uri);
                Console.ReadLine();
            }
        }
    }
}
