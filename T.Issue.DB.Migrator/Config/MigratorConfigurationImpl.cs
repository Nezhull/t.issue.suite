using System.Collections.Generic;
using System.Reflection;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;

namespace T.Issue.DB.Migrator.Config
{
    internal class MigratorConfigurationImpl : IMigratorConfiguration
    {
        private const string DefaultScriptsLocation = "Scripts";
        private const string DefaultVersionTable = "Schema_Version";

        public int CommandTimeout { get; set; }
        public string SchemaVersionTable { get; set; }
        public bool HaltOnValidationError { get; set; }

        public IList<ItemLocation> ScriptsLocations { get; }
        public IDictionary<string, object> Parameters { get; }

        public MigratorConfigurationImpl(Assembly assembly) : this(new ItemLocation { Assembly = assembly, Location = DefaultScriptsLocation })
        {
        }

        public MigratorConfigurationImpl(Assembly assembly, string scriptsLocation) : this(new ItemLocation { Assembly = assembly, Location = scriptsLocation })
        {
        }

        public MigratorConfigurationImpl(ItemLocation location) : this(new List<ItemLocation>(new[] { location }))
        {
        }

        public MigratorConfigurationImpl(IList<ItemLocation> locations)
        {
            Assert.IsNotEmpty(locations);

            ScriptsLocations = new List<ItemLocation>(locations);
            Parameters = new Dictionary<string, object>();
            SchemaVersionTable = DefaultVersionTable;
            HaltOnValidationError = false;
            CommandTimeout = 120;
        }

        public IMigratorConfiguration AddScriptsLocation(Assembly assembly, string scriptsLocation)
        {
            ScriptsLocations.Add(new ItemLocation
            {
                Assembly = assembly,
                Location = scriptsLocation
            });
            return this;
        }

        public IMigratorConfiguration AddParameter(string key, object value)
        {
            Parameters.Add(key, value);
            return this;
        }

        public IMigratorConfiguration SetCommandTimeout(int commandTimeout)
        {
            CommandTimeout = commandTimeout;
            return this;
        }

        public IMigratorConfiguration SetSchemaVersionTable(string schemaVersionTable)
        {
            SchemaVersionTable = schemaVersionTable;
            return this;
        }

        public IMigratorConfiguration SetHaltOnValidationError(bool haltOnValidationError)
        {
            HaltOnValidationError = haltOnValidationError;
            return this;
        }
    }
}
