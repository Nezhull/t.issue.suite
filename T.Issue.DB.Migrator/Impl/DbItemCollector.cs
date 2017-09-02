using System.Collections.Generic;
using System.Data.SqlClient;
using T.Issue.Bootstrapper.Collectors;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;

namespace T.Issue.DB.Migrator.Impl
{
    public class DbItemCollector : IBootstrapItemCollector<AppliedItem>
    {
        private readonly IMigratorConfiguration configuration;
        private readonly IDbAccessFacade dbAccessFacade;
        private readonly SqlConnection connection;

        public DbItemCollector(SqlConnection connection, IMigratorConfiguration configuration, IDbAccessFacade dbAccessFacade)
        {
            Assert.NotNull(connection);
            Assert.NotNull(configuration);
            Assert.NotNull(dbAccessFacade);

            this.configuration = configuration;
            this.dbAccessFacade = dbAccessFacade;
            this.connection = connection;
        }

        public IList<AppliedItem> Collect()
        {
            return dbAccessFacade.GetAppliedScripts(connection, configuration);
        }
    }
}