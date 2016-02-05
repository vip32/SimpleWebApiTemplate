using System;
using ExtendedApi;
using Microsoft.Owin.Hosting;

namespace ExtendedConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            const string uri = "http://localhost:8601";
            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine(uri);
                Console.ReadLine();
            }
        }
    }
}
