namespace ScriptCs.Dnx.Contracts
{
    public interface IScriptHostFactory
    {
        IScriptHost CreateScriptHost(IScriptPackManager scriptPackManager, string[] scriptArgs);
    }
}