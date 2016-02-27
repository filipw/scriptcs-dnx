namespace ScriptCs.Dnx.Command
{
    public interface IScriptCommand : ICommand
    {
        string[] ScriptArgs { get; }
    }
}