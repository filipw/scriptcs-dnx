namespace ScriptCs.Dnx.Contracts
{
    public interface IScriptHost
    {
        T Require<T>() where T : IScriptPackContext;
        IScriptEnvironment Env { get; }
    }
}