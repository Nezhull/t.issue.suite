using Microsoft.VisualStudio.TestTools.UnitTesting;
using T.Issue.DB.Migrator;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using T.Issue.Bootstrapper.Model;
using T.Issue.DB.Migrator.Impl;
using T.Issue.DB.MigratorTests;
using log4net.Config;
using T.Issue.DB.Migrator.Config;

namespace T.Issue.DB.Migrator
{
    [TestClass]
    public class RepeatableScriptsTests
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
            config = MigratorConfigurationBuilder.Build(Assembly.GetExecutingAssembly(), "TestRepeatableScripts")
                .SetHaltOnValidationError(true);
        }

        [TestMethod]
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

            Assert.IsTrue(dbAccessFacade.DbItems.Count == 1);
            Assert.IsTrue(dbAccessFacade.AppliedItems.Count == 1);
        }

        [TestMethod]
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

            Assert.IsTrue(dbAccessFacade.DbItems.Count == 1);
            Assert.IsTrue(dbAccessFacade.AppliedItems.Count == 0);
        }
    }
}