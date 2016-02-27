using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Core
{
    public interface IShebangLineProcessor : ILineProcessor
    {
    }

    public class ShebangLineProcessor : DirectiveLineProcessor, IShebangLineProcessor
    {
        protected override string DirectiveName
        {
            get { return "!/usr/bin/env"; }
        }

        protected override bool ProcessLine(IFileParser parser, FileParserContext context, string line)
        {
            return true;
        }
    }
}