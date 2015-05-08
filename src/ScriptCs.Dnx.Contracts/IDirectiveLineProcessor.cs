namespace ScriptCs.Dnx.Contracts
{
    public interface IDirectiveLineProcessor : ILineProcessor
    {
        bool Matches(string line);
    }
}