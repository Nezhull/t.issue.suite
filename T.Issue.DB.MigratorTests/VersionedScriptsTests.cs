using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using T.Issue.Bootstrapper.Model;
using T.Issue.DB.MigratorTests;
using log4net.Config;
using T.Issue.DB.Migrator.Config;

namespace T.Issue.DB.Migrator
{
    [TestClass]
    public class VersionedScriptsTests
    {
        private IMigratorConfiguration config;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            XmlConfigurator.Configure();
        }

        [TestInitialize]
        public void Initialize()
        {
            config = MigratorConfigurationBuilder.Build(Assembly.GetExecutingAssembly(), "TestScripts")
                .AddScriptsLocation(Assembly.GetExecutingAssembly(), "TestScripts1")
                .SetHaltOnValidationError(true);
        }

        [TestMethod]
        public void SetupDbMigrationTest()
        {
            DbAccessFacadeMock dbAccessFacade = new DbAccessFacadeMock();

            IDatabaseMigrator migrator = DatabaseMigratorBuilder.Build(config, dbAccessFacade);

            migrator.Migrate(new SqlConnection());

            Assert.IsTrue(dbAccessFacade.AppliedItems.Count == 5);
        }

        [TestMethod]
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

            Assert.IsTrue(dbAccessFacade.DbItems.Count == 5);
            Assert.IsTrue(dbAccessFacade.AppliedItems.Count == 4);
        }

        [TestMethod]
        public void ParametersTest()
        {
            DbAccessFacadeMock dbAccessFacade = new DbAccessFacadeMock();

            config.AddParameter("parameters_test_key_1", "col1");
            config.AddParameter("parameters_test_key_2", "col2");

            IDatabaseMigrator migrator = DatabaseMigratorBuilder.Build(config, dbAccessFacade);

            migrator.Migrate(new SqlConnection());

            ClasspathItem script = dbAccessFacade.ClasspathScripts.First(s => s.Version.Equals("201606201610"));
            Assert.IsNotNull(script);

            Assert.IsTrue(script.Content.ToString().Contains("[col1]"));
            Assert.IsTrue(script.Content.ToString().Contains("[col2]"));
            Assert.IsFalse(script.Content.ToString().Contains("{{parameters_test_key_1}}"));
            Assert.IsFalse(script.Content.ToString().Contains("{{parameters_test_key_2}}"));
        }

        [TestMethod]
        public void ParametersAndFiltersTest()
        {
            DbAccessFacadeMock dbAccessFacade = new DbAccessFacadeMock();

            config.AddScriptsLocation(Assembly.GetExecutingAssembly(), "TemplateTestScripts");

            IDatabaseMigrator migrator = DatabaseMigratorBuilder.Build(config, dbAccessFacade);

            migrator.Migrate(new SqlConnection());

            ClasspathItem script = dbAccessFacade.ClasspathScripts.First(s => s.Version.Equals("201701171635"));
            Assert.IsNotNull(script);

            Assert.IsTrue(script.Content.ToString().Contains("0x"));
        }

        [TestMethod]
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

            Assert.IsTrue(dbAccessFacade.DbItems.Count == 1);
            Assert.IsTrue(dbAccessFacade.AppliedItems.Count == 0);
        }

        [TestMethod]
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

            Assert.IsTrue(dbAccessFacade.DbItems.Count == 1);
            Assert.IsTrue(dbAccessFacade.AppliedItems.Count == 0);
        }
    }
}