using System;

namespace ScriptCs.Dnx.Core
{
    public class ScriptCompilationException : Exception
    {
        public ScriptCompilationException(string message)
            : base(message)
        {
        }

        public ScriptCompilationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        //todo
        //protected ScriptCompilationException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }
}