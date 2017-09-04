using System;
using System.Data.SqlClient;
using System.Reflection;
using Common.Logging;
using T.Issue.Bootstrapper.Model;
using T.Issue.DB.Migrator.Config;
using T.Issue.DB.Migrator.Test.Logging;
using Xunit;
using Xunit.Abstractions;

namespace T.Issue.DB.Migrator.Test
{
    public class RepeatableScriptsTests
    {
        private readonly IMigratorConfiguration config;

        public RepeatableScriptsTests(ITestOutputHelper output)
        {
            LogManager.Adapter = new XunitLoggerFactoryAdapter(LogLevel.Debug, output);

            config = MigratorConfigurationBuilder.Build(Assembly.GetExecutingAssembly(), "TestRepeatableScripts")
                .SetHaltOnValidationError(true);
        }

        [Fact]
        public void RepeatableScriptsReapplyTest()
        {
            DbAccessFacadeMock dbAccessFacade = new DbAccessFacadeMock();
            dbAccessFacade.DbItems.Add(new AppliedItem
            {
                Id = "201606201701",
                Type = ItemType.Repeatable,
                Version = "201606201701",
                Name = "R2016.06.20_1701_repeatable_test2.sql",
                Executed = DateTime.Now,
                Checksum = "2e542df38430899677f99ce8be5b97a7"
            });

            IDatabaseMigrator migrator = DatabaseMigratorBuilder.Build(config, dbAccessFacade);

            migrator.Migrate(new SqlConnection());

            Assert.True(dbAccessFacade.DbItems.Count == 1);
            Assert.True(dbAccessFacade.AppliedItems.Count == 1);
        }

        [Fact]
        public void RepeatableScriptsNoReapplyTest()
        {
            DbAccessFacadeMock dbAccessFacade = new DbAccessFacadeMock();
            dbAccessFacade.DbItems.Add(new AppliedItem
            {
                Id = "201606201701",
                Type = ItemType.Repeatable,
                Version = "201606201701",
                Name = "R2016.06.20_1701_repeatable_test2.sql",
                Executed = DateTime.Now,
                Checksum = "470f6a50c4fd0323b2dc95be00ab72e1"
            });

            IDatabaseMigrator migrator = DatabaseMigratorBuilder.Build(config, dbAccessFacade);

            migrator.Migrate(new SqlConnection());

            Assert.True(dbAccessFacade.DbItems.Count == 1);
            Assert.True(dbAccessFacade.AppliedItems.Count == 0);
        }
    }
}