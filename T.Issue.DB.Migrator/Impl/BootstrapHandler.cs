using System.Data.SqlClient;
using System.Linq;
using Common.Logging;
using T.Issue.Bootstrapper;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;

namespace T.Issue.DB.Migrator.Impl
{
    internal class BootstrapHandler : IBootstrapHandler<PendingItem>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BootstrapHandler));

        private readonly IMigratorConfiguration configuration;
        private readonly IDbAccessFacade dbAccessFacade;
        private readonly SqlConnection connection;

        public BootstrapHandler(SqlConnection connection, IMigratorConfiguration configuration, IDbAccessFacade dbAccessFacade)
        {
            Assert.NotNull(connection);
            Assert.NotNull(configuration);
            Assert.NotNull(dbAccessFacade);

            this.configuration = configuration;
            this.dbAccessFacade = dbAccessFacade;
            this.connection = connection;
        }

        public void Init()
        {
            Assert.IsNotEmpty(configuration.ScriptsLocations);
            Assert.HasText(configuration.SchemaVersionTable);

            Log.Info("Starting DB migration from locations:");
            foreach (var location in configuration.ScriptsLocations)
            {
                Log.InfoFormat("Location: {0}, assembly: {1}", location.Location, location.Assembly.GetName());
            }

            dbAccessFacade.CreateSchemaTableIfNotExist(connection, configuration);
        }

        public void Bootstrap(IOrderedEnumerable<PendingItem> pendingItems)
        {
            dbAccessFacade.ApplyPendingScripts(connection, configuration, pendingItems);
        }
    }
}
