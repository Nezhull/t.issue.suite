using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T.Issue.DB.Migrator;
using T.Issue.DB.Migrator.Impl;
using System.Data.SqlClient;
using T.Issue.Bootstrapper.Model;
using log4net;

namespace T.Issue.DB.MigratorTests
{
    class DbAccessFacadeMock : IDbAccessFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DbAccessFacadeMock));

        public IList<AppliedItem> DbItems { get; }
        public IList<AppliedItem> AppliedItems { get; }
        public IList<ClasspathItem> ClasspathScripts { get; }

        public DbAccessFacadeMock()
        {
            DbItems = new List<AppliedItem>();
            AppliedItems = new List<AppliedItem>();
            ClasspathScripts = new List<ClasspathItem>();
        }

        public void CreateSchemaTableIfNotExist(SqlConnection connection, IMigratorConfiguration configuration)
        {
        }

        public IList<AppliedItem> GetAppliedScripts(SqlConnection connection, IMigratorConfiguration configuration)
        {
            return DbItems;
        }

        public void ApplyPendingScripts(SqlConnection connection, IMigratorConfiguration configuration, IEnumerable<ClasspathItem> scripts)
        {
            foreach (var updateScript in scripts)
            {
                ClasspathScripts.Add(updateScript);

                AppliedItem appliedScript = new AppliedItem
                {
                    Type = updateScript.Type,
                    Executed = DateTime.Now,
                    Name = updateScript.Name,
                    Version = updateScript.Version,
                    Checksum = updateScript.Checksum
                };

                if (updateScript.Type == ItemType.Repeatable)
                {
                    AppliedItem oldItem = DbItems.FirstOrDefault(s => Equals(updateScript.Version, s.Version));
                    if (oldItem != null)
                    {
                        DbItems.Remove(oldItem);
                    }
                }

                DbItems.Add(appliedScript);
                AppliedItems.Add(appliedScript);

                Log.InfoFormat("Running script {0} with version {1}", updateScript.Name, updateScript.Version);
            }
        }
    }
}
