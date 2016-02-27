using System;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Hosting
{
    //todo: add the mono line editor
    public class ScriptConsole : IConsole
    {
        public void Write(string value)
        {
            Console.Write(value);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public string ReadLine(string prompt)
        {
            return Console.ReadLine();
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void Exit()
        {
            ResetColor();
            Environment.FailFast("Ciao!");
        }

        public void ResetColor()
        {
            Console.ResetColor();
        }

        public ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }
    }
}