using System;
using System.IO;
using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Hosting
{
    public class FileConsole : IConsole
    {
        private readonly string _path;
        private readonly IConsole _innerConsole;

        public FileConsole(string path, IConsole innerConsole)
        {
            if (innerConsole == null) throw new ArgumentNullException(nameof(innerConsole));

            _path = path;
            _innerConsole = innerConsole;
        }

        public void Write(string value)
        {
            _innerConsole.Write(value);
            this.Append(value);
        }

        public void WriteLine()
        {
            _innerConsole.WriteLine();
            this.AppendLine(string.Empty);
        }

        public void WriteLine(string value)
        {
            _innerConsole.WriteLine(value);
            this.AppendLine(value);
        }

        public string ReadLine()
        {
            var line = _innerConsole.ReadLine();
            this.AppendLine(line);
            return line;
        }

        public void Clear()
        {
            _innerConsole.Clear();
        }

        public void Exit()
        {
            _innerConsole.Exit();
        }

        public void ResetColor()
        {
            _innerConsole.ResetColor();
        }

        public ConsoleColor ForegroundColor
        {
            get { return _innerConsole.ForegroundColor; }
            set { _innerConsole.ForegroundColor = value; }
        }

        private void Append(string text)
        {
            using(var fs = new FileStream(_path, FileMode.Append))
            using (var writer = new StreamWriter(fs))
            {
                writer.Write(text);
                writer.Flush();
            }
        }

        private void AppendLine(string text)
        {
            this.Append(text + Environment.NewLine);
        }
    }
}