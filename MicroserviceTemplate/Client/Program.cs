using System;

using Infrastructure;

using Service;

namespace Client
{
    class Program
    {
        static void Main()
        {
            var topic = new Topic("health", "health");
            var topic2 = new Topic("healthTyped", "healthTyped");
            topic.Subscribe((x)=>Console.WriteLine("Got: "+x));
            topic2.Subscribe<HealthMessage>((x)=>Console.WriteLine("Got typed: "+x.WhoIsCalling));
            Console.ReadLine();
        }
    }
}
