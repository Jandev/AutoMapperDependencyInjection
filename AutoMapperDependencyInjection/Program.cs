using System;
using Autofac;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            var container = Registration.Autofac();

            using (var scope = container.BeginLifetimeScope())
            {
                var startup = scope.Resolve<IStartup>();
                startup.Start();
            }

            Console.ReadLine();
        }
    }
}
