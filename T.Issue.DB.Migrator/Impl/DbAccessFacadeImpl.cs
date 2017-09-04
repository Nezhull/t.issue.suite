using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using Common.Logging;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;
using T.Issue.DB.Migrator.Utils;

namespace T.Issue.DB.Migrator.Impl
{
    internal class DbAccessFacadeImpl : IDbAccessFacade
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DbAccessFacadeImpl));

        // TODO Use DotLiquid templates
        private const string ParameterPrefix = "${";
        private const string ParameterPostfix = "}";
        private const string SchemaVersionTableParam = ParameterPrefix + "schema.version.table" + ParameterPostfix;

        private const string SelectSchemaVersionObjectIdSql = "select Object_id('{0}')";
        private const string SelectFromSchemaVersionSql = "select [Type], [Version], [Script], [Executed], [Hash] from [{0}]";
        private const string SelectSingleFromSchemaVersionSql = "select [Type], [Version], [Script], [Executed], [Hash] from [{0}] with (TABLOCKX, HOLDLOCK) where [Version] = @Version";
        private const string InsertIntoSchemaVersionSql = "insert into {0}([Type], [Version], [Script], [Executed], [Hash]) values (@Type, @Version, @Script, @Executed, @Hash)";
        private const string UpdateSchemaVersionHashSql = "update {0} set [Hash] = @Hash, [Executed] = @Executed where [Version] = @Version";

        public void CreateSchemaTableIfNotExist(SqlConnection connection, IMigratorConfiguration configuration)
        {
            using (var tx = connection.BeginTransaction())
            {
                if (SchemaTableExists(connection, tx, configuration))
                {
                    Log.Debug("Schema table exists.");
                    return;
                }
                Log.Debug("Schema table is not found, creating it now...");

#if NETSTANDARD1_3
                Assembly assembly = typeof(DbAccessFacadeImpl).GetTypeInfo().Assembly;
                using (var stream = assembly.GetManifestResourceStream(GetType().Namespace + ".create.sql"))
#elif NET40 || NET452
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream(GetType(), "create.sql"))
#endif
                {
                    Assert.NotNull(stream, "Could not locate create.sql resource");
                    foreach (var item in GetSchemaVersionTableScripts(stream, configuration))
                    {
                        using (var command = new SqlCommand(item, connection, tx))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                tx.Commit();
            }
            Log.Debug("Schema table created.");
        }

        public IList<AppliedItem> GetAppliedScripts(SqlConnection connection, IMigratorConfiguration configuration)
        {
            List<AppliedItem> result = new List<AppliedItem>();
            using (var command = new SqlCommand(string.Format(SelectFromSchemaVersionSql, configuration.SchemaVersionTable), connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(BuildAppliedScript(reader));
                    }
                }
            }
            return result;
        }

        public void ApplyPendingScripts(SqlConnection connection, IMigratorConfiguration configuration, IEnumerable<PendingItem> scripts)
        {
            foreach (var script in scripts)
            {
                using (var tx = connection.BeginTransaction())
                {
                    RunScript(connection, tx, configuration, script);
                    tx.Commit();
                }
            }
        }

        private void RunScript(SqlConnection connection, SqlTransaction tx, IMigratorConfiguration configuration, PendingItem script)
        {
            Log.DebugFormat(script.Type == ItemType.Versioned ? "Applying script {0}" : "Reapplying script {0}", script.Name);

            AppliedItem appliedScript = GetAppliedScript(connection, tx, configuration, script);
            if (appliedScript != null && appliedScript.Type == ItemType.Versioned)
            {
                Log.Debug("Script already applied, skipping.");
                return;
            }

            foreach (var scriptItem in new ScriptSplitter(script.Content.ToString()))
            {
                try
                {
                    using (var command = new SqlCommand(scriptItem, connection, tx))
                    {
                        command.CommandTimeout = configuration.CommandTimeout;
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException)
                {
                    Log.Error("Error while running script:\n" + scriptItem);
                    throw;
                }
            }

            if (appliedScript == null)
            {
                InsertIntoSchemaVersion(connection, tx, configuration, script);
            }
            else
            {
                UpdateSchemaVersion(connection, tx, configuration, script);
            }
        }

        private void InsertIntoSchemaVersion(SqlConnection connection, SqlTransaction tx, IMigratorConfiguration configuration, PendingItem script)
        {
            var insertSql = string.Format(InsertIntoSchemaVersionSql, configuration.SchemaVersionTable);

            using (var command = new SqlCommand(insertSql, connection, tx))
            {
                command.Parameters.Add(new SqlParameter("Type", ScriptTypeUtils.ResolveString(script.Type)));
                command.Parameters.Add(new SqlParameter("Version", script.Version));
                command.Parameters.Add(new SqlParameter("Script", script.Name));
                command.Parameters.Add(new SqlParameter("Executed", DateTime.Now));
                command.Parameters.Add(new SqlParameter("Hash", script.Checksum));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateSchemaVersion(SqlConnection connection, SqlTransaction tx, IMigratorConfiguration configuration, PendingItem script)
        {
            var updateSql = string.Format(UpdateSchemaVersionHashSql, configuration.SchemaVersionTable);

            using (var command = new SqlCommand(updateSql, connection, tx))
            {
                command.Parameters.Add(new SqlParameter("Hash", script.Checksum));
                command.Parameters.Add(new SqlParameter("Executed", DateTime.Now));
                command.Parameters.Add(new SqlParameter("Version", script.Version));
                command.ExecuteNonQuery();
            }
        }

        private bool SchemaTableExists(SqlConnection connection, SqlTransaction tx, IMigratorConfiguration configuration)
        {
            using (var command = new SqlCommand(string.Format(SelectSchemaVersionObjectIdSql, configuration.SchemaVersionTable), connection, tx))
            {
                using (var reader = command.ExecuteReader())
                {
                    return (reader.Read() && !reader.IsDBNull(0));
                }
            }
        }

        public AppliedItem GetAppliedScript(SqlConnection connection, SqlTransaction tx, IMigratorConfiguration configuration, PendingItem script)
        {
            AppliedItem result = null;

            using (var command = new SqlCommand(string.Format(SelectSingleFromSchemaVersionSql, configuration.SchemaVersionTable), connection, tx))
            {
                command.CommandTimeout = configuration.CommandTimeout;
                command.Parameters.Add(new SqlParameter("Version", script.Version));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = BuildAppliedScript(reader);
                    }
                }
            }

            return result;
        }

        private AppliedItem BuildAppliedScript(SqlDataReader reader)
        {
            ItemType type = ScriptTypeUtils.ResolveType(reader.GetString(0));
            string version = reader.GetString(1);

            return new AppliedItem
            {
                Id = version,
                Type = type,
                Version = version,
                Name = reader.GetString(2),
                Executed = reader.GetDateTime(3),
                Checksum = reader.GetString(4)
            };
        }

        private IEnumerable<string> GetSchemaVersionTableScripts(Stream stream, IMigratorConfiguration configuration)
        {
            Assert.NotNull(stream, "Could not locate create.sql resource");

            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var createScript = reader.ReadToEnd();
                if (createScript.Contains(SchemaVersionTableParam))
                {
                    createScript = createScript.Replace(SchemaVersionTableParam, configuration.SchemaVersionTable);
                }

                return new ScriptSplitter(createScript);
            }
        }
    }
}
