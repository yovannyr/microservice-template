using System;
using Microsoft.Owin.Hosting;

namespace Service
{
    class Program
    {
        static void Main()
        {
            var url = "http://+:1234";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Running on {0}. {1}", url, "Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
