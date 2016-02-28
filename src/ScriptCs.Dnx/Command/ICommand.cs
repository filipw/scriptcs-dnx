namespace ScriptCs.Dnx.Command
{
    public interface ICommand
    {
        CommandResult Execute();
    }

    public interface IExecuteReplCommand : IScriptCommand
    {
    }
}