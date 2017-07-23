using System.Collections.Generic;
using System.Data.SqlClient;
using T.Issue.Bootstrapper.Model;

namespace T.Issue.DB.Migrator
{
    public interface IDbAccessFacade
    {
        void CreateSchemaTableIfNotExist(SqlConnection connection, IMigratorConfiguration configuration);

        IList<AppliedItem> GetAppliedScripts(SqlConnection connection, IMigratorConfiguration configuration);

        void ApplyPendingScripts(SqlConnection connection, IMigratorConfiguration configuration, IEnumerable<ClasspathItem> scripts);
    }
}
