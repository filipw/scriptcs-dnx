namespace ScriptCs.Dnx.Contracts
{
    public interface IScriptPackManager
    {
        TContext Get<TContext>() where TContext : IScriptPackContext;
    }
}