using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Common.Logging;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;
using T.Issue.DB.Migrator.Config;
using T.Issue.DB.Migrator.Test.Logging;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace T.Issue.DB.Migrator.Test
{
    public class VersionedScriptsTests
    {
        private readonly IMigratorConfiguration config;

        public VersionedScriptsTests(ITestOutputHelper output)
        {
            LogManager.Adapter = new XunitLoggerFactoryAdapter(LogLevel.Debug, output);

            Assembly assembly = ReflectionUtils.GetAssembly<VersionedScriptsTests>();
            config = MigratorConfigurationBuilder.Build(assembly, "TestScripts")
                .AddScriptsLocation(assembly, "TestScripts1")
                .SetHaltOnValidationError(true);
        }

        [Fact]
        public void SetupDbMigrationTest()
        {
            DbAccessFacadeMock dbAccessFacade = new DbAccessFacadeMock();

            IDatabaseMigrator migrator = DatabaseMigratorBuilder.Build(config, dbAccessFacade);

            migrator.Migrate(new SqlConnection());

            Assert.True(dbAccessFacade.AppliedItems.Count == 5);
        }

        [Fact]
        public void UpdateSchemaTest()
        {
            DbAccessFacadeMock dbAccessFacade = new DbAccessFacadeMock();
            dbAccessFacade.DbItems.Add(new AppliedItem
            {
                Id = "201606201607",
                Type = ItemType.Versioned,
                Version = "201606201607",
                Name = "V2016.06.20_1607_test1.sql",
                Executed = DateTime.Now,
                Checksum = "2e542df38430899677f99ce8be5b97a6"
            });

            IDatabaseMigrator migrator = DatabaseMigratorBuilder.Build(config, dbAccessFacade);

            migrator.Migrate(new SqlConnection());

            Assert.True(dbAccessFacade.DbItems.Count == 5);
            Assert.True(dbAccessFacade.AppliedItems.Count == 4);
        }

        [Fact]
        public void ParametersTest()
        {
            DbAccessFacadeMock dbAccessFacade = new DbAccessFacadeMock();

            config.AddParameter("parameters_test_key_1", "col1");
            config.AddParameter("parameters_test_key_2", "col2");

            IDatabaseMigrator migrator = DatabaseMigratorBuilder.Build(config, dbAccessFacade);

            migrator.Migrate(new SqlConnection());

            PendingItem script = dbAccessFacade.ClasspathScripts.First(s => s.Version.Equals("201606201610"));
            Assert.NotNull(script);

            Assert.True(script.Content.ToString().Contains("[col1]"));
            Assert.True(script.Content.ToString().Contains("[col2]"));
            Assert.False(script.Content.ToString().Contains("{{parameters_test_key_1}}"));
            Assert.False(script.Content.ToString().Contains("{{parameters_test_key_2}}"));
        }

        [Fact]
        public void ParametersAndFiltersTest()
        {
            DbAccessFacadeMock dbAccessFacade = new DbAccessFacadeMock();

            Assembly assembly = ReflectionUtils.GetAssembly<VersionedScriptsTests>();

            config.AddScriptsLocation(assembly, "TemplateTestScripts");

            IDatabaseMigrator migrator = DatabaseMigratorBuilder.Build(config, dbAccessFacade);

            migrator.Migrate(new SqlConnection());

            PendingItem script = dbAccessFacade.ClasspathScripts.First(s => s.Version.Equals("201701171635"));
            Assert.NotNull(script);

            Assert.True(script.Content.ToString().Contains("0x"));
        }

        [Fact]
        public void UpdateSchemaHashFailTest()
        {
            DbAccessFacadeMock dbAccessFacade = new DbAccessFacadeMock();
            dbAccessFacade.DbItems.Add(new AppliedItem
            {
                Id = "201606201607",
                Type = ItemType.Versioned,
                Version = "201606201607",
                Name = "V2016.06.20_1607_test1.sql",
                Executed = DateTime.Now,
                Checksum = "2e542df38430899677f99ce8be5b97a7"
            });

            IDatabaseMigrator migrator = DatabaseMigratorBuilder.Build(config, dbAccessFacade);

            migrator.Migrate(new SqlConnection());

            Assert.True(dbAccessFacade.DbItems.Count == 1);
            Assert.True(dbAccessFacade.AppliedItems.Count == 0);
        }

        [Fact]
        public void UpdateSchemaNoUpdateFileTest()
        {
            DbAccessFacadeMock dbAccessFacade = new DbAccessFacadeMock();
            dbAccessFacade.DbItems.Add(new AppliedItem
            {
                Id = "201606201606",
                Type = ItemType.Versioned,
                Version = "201606201606",
                Name = "V2016.06.20_1606_test0.sql",
                Executed = DateTime.Now,
                Checksum = "2e542df38430899677f99ce8be5b97a7"
            });

            IDatabaseMigrator migrator = DatabaseMigratorBuilder.Build(config, dbAccessFacade);

            migrator.Migrate(new SqlConnection());

            Assert.True(dbAccessFacade.DbItems.Count == 1);
            Assert.True(dbAccessFacade.AppliedItems.Count == 0);
        }
    }
}