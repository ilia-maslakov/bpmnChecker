using System;

namespace BpmnParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Environment.Exit(ProgramRunner.Run(args, Console.Out));
        }
    }
}
