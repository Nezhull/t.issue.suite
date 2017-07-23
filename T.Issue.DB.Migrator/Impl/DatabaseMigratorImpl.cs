using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using T.Issue.Bootstrapper;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;
using log4net;

namespace T.Issue.DB.Migrator.Impl
{
    internal class DatabaseMigratorImpl : IDatabaseMigrator
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(DatabaseMigratorImpl));

        private readonly IMigratorConfiguration configuration;
        private readonly IDbAccessFacade dbAccessFacade;
        private readonly IBootstrapper<AppliedItem, ClasspathItem> bootstrapper = BootstrapperBuilder.Build<AppliedItem, ClasspathItem>();

        public DatabaseMigratorImpl(IMigratorConfiguration configuration) : this(configuration, new DbAccessFacadeImpl())
        {
        }

        public DatabaseMigratorImpl(IMigratorConfiguration configuration, IDbAccessFacade dbAccessFacade)
        {
            this.configuration = configuration;
            this.dbAccessFacade = dbAccessFacade;
        }

        public void Migrate(SqlConnection connection)
        {
            Assert.NotNull(connection);

            BootstrapHandler handler = new BootstrapHandler(connection, configuration, dbAccessFacade);

            handler.Init();

            LOG.Info(bootstrapper.Bootstrap(handler) ? "Migration completed." : "Migration terminated!");
        }
    }
}