using ScriptCs.Dnx.Contracts;

namespace ScriptCs.Dnx.Hosting
{
    public class NullFileSystemMigrator : IFileSystemMigrator
    {
        public void Migrate()
        {
            return;
        }
    }
}