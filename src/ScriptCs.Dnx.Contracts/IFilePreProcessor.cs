namespace ScriptCs.Dnx.Contracts
{
    public interface IFilePreProcessor : IFileParser
    {
        FilePreProcessorResult ProcessFile(string path);

        FilePreProcessorResult ProcessScript(string script);
    }
}