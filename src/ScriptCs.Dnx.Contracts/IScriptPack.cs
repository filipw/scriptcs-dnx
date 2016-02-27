namespace ScriptCs.Dnx.Contracts
{
    public interface IScriptPack
    {
        void Initialize(IScriptPackSession session);

        IScriptPackContext GetContext();

        void Terminate();
    }

    public interface IScriptPack<TContext> : IScriptPack where TContext : IScriptPackContext
    {
        TContext Context { get; set; }
    }
}