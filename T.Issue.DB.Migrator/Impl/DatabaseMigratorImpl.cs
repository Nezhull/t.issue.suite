using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DotLiquid;
using DotLiquid.NamingConventions;
using T.Issue.Bootstrapper;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;
using log4net;
using T.Issue.Bootstrapper.Impl;

namespace T.Issue.DB.Migrator.Impl
{
    internal class DatabaseMigratorImpl : IDatabaseMigrator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DatabaseMigratorImpl));

        private readonly IMigratorConfiguration configuration;
        private readonly IDbAccessFacade dbAccessFacade;

        public DatabaseMigratorImpl(IMigratorConfiguration configuration) : this(configuration, new DbAccessFacadeImpl())
        {
        }

        public DatabaseMigratorImpl(IMigratorConfiguration configuration, IDbAccessFacade dbAccessFacade)
        {
            this.configuration = configuration;
            this.dbAccessFacade = dbAccessFacade;
            
            Template.RegisterFilter(typeof(BinaryToHexFilter));
            Template.NamingConvention = new CSharpNamingConvention();
        }

        public void Migrate(SqlConnection connection)
        {
            Assert.NotNull(connection);

            ClasspathItemCollector classpathItemCollector = new ClasspathItemCollector(configuration.ScriptsLocations, configuration.Parameters);
            DbItemCollector dbItemCollector = new DbItemCollector(connection, configuration, dbAccessFacade);
            BootstrapHandler handler = new BootstrapHandler(connection, configuration, dbAccessFacade);
            
            IBootstrapper<AppliedItem,PendingItem> bootstrapper = new BootstrapperImpl<AppliedItem,PendingItem>(handler, classpathItemCollector, dbItemCollector);
            bootstrapper.OnItemsCollected = OnItemsCollected;
            bootstrapper.OnValidationError = OnValidationError;
            bootstrapper.HaltOnValidationError = configuration.HaltOnValidationError;

            handler.Init();
            
            Log.Info(bootstrapper.Bootstrap() ? "Migration completed." : "Migration terminated!");
        }

        private static void OnValidationError(ValidationContext<AppliedItem, PendingItem> ctx)
        {
            switch (ctx.ErrorType)
            {
                case ErrorType.MissingAppliedClasspathItem:
                    Log.ErrorFormat("Unable to find schema migration script {0} hash {1} applied at {2}!", ctx.AppliedItem.Name, ctx.AppliedItem.Checksum, ctx.AppliedItem.Executed);
                    break;

                case ErrorType.AppliedAndClasspathItemChecksumMismatch:
                    Log.ErrorFormat("Applied versioned migration script {0} hash {1} is different from file hash {2}!", ctx.AppliedItem.Name, ctx.AppliedItem.Checksum, ctx.PendingItem.Checksum);
                    break;
                default:
                    Log.ErrorFormat("Validation error for script {0}", ctx.AppliedItem.Name);
                    break;
            }
        }

        private static void OnItemsCollected(IList<PendingItem> pendingItems, IList<AppliedItem> appliedItems)
        {
            string currentVersion = appliedItems.Where(i => i.Type == ItemType.Versioned).Select(i => i.Version).Max(i => i);
            string repeatableVersion = appliedItems.Where(i => i.Type == ItemType.Repeatable).Select(i => i.Version).Max(i => i);

            if (currentVersion != null)
            {
                string version = $"Current schema version: {currentVersion}";

                if (repeatableVersion != null)
                {
                    version = $"{version} ({repeatableVersion})";
                }

                Log.InfoFormat(version);
            }
        }
    }
}