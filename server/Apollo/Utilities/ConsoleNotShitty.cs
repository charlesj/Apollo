using System;
using System.Collections.Generic;
using System.Linq;

namespace Apollo.Utilities
{
    public class ConsoleNotShitty
    {
        public void Write(string output, bool newLine = true, ConsoleColor color = ConsoleColor.Gray)
        {
            var currentConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (newLine)
                Console.WriteLine(output);
            else
                Console.Write(output);

            Console.ForegroundColor = currentConsoleColor;
        }

        public void Red(string output, bool newLine = true)
        {
            Write(output, newLine, ConsoleColor.Red);
        }

        public void Yellow(string output, bool newLine = true)
        {
            Write(output, newLine, ConsoleColor.Yellow);
        }

        public void Green(string output, bool newLine = true)
        {
            Write(output, newLine, ConsoleColor.Green);
        }

        public string ReadLineSupressOutput()
        {
            var line = new Stack<char>();
            var key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace)
                {
                    if(line.Count > 0)
                        line.Pop();
                    continue;
                }

                line.Push(key.KeyChar);
                key = Console.ReadKey(true);
            }

            Console.WriteLine(string.Empty);

            return new string(line.Reverse().ToArray());
        }
    }
}