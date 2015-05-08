using System;

namespace ScriptCs.Dnx.Contracts
{
    public class InvalidDirectiveUseException : Exception
    {
        public InvalidDirectiveUseException(string message)
            : base(message)
        {
        }

        public InvalidDirectiveUseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}