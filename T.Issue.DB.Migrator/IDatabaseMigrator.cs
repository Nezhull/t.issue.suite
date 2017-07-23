using System.Data.SqlClient;

namespace T.Issue.DB.Migrator
{
    public interface IDatabaseMigrator
    {
        void Migrate(SqlConnection connection);
    }
}