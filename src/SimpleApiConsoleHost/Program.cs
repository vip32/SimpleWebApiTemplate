using System;
using Microsoft.Owin.Hosting;
using SimpleApi;

namespace SimpleApiConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:12345"))
            {
                Console.ReadLine();
            }
        }
    }
}
