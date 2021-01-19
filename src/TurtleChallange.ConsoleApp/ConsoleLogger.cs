using System;
using TurtleChallange.Logic;

namespace TurtleChallange.ConsoleApp
{
    class ConsoleLogger : IExternalLogger
    {
        public void Log(string contents)
        {
            Console.WriteLine(contents);
        }
    }
}
