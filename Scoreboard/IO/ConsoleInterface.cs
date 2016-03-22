namespace Scoreboard.IO
{
    using System;
    using Interfaces;

    public class ConsoleInterface : IIOInterface
    {
        public void Write(string text, params object[] @params)
        {
            Console.WriteLine(text, @params);
        }

        public string Read()
        {
            var text = Console.ReadLine();

            return text;
        }
    }
}