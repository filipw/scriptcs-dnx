using System;

namespace ScriptCs.Dnx.Contracts
{
    public delegate bool Logger(LogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters);
}