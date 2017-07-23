using System.Collections.Generic;
using System.Reflection;
using T.Issue.Bootstrapper.Model;

namespace T.Issue.DB.Migrator
{
    /// <summary>
    /// Configuration object for DB migrator.
    /// </summary>
    public interface IMigratorConfiguration
    {
        /// <summary>
        /// DB migration single command timeout in seconds, default 120 seconds.
        /// </summary>
        int CommandTimeout { get; }

        /// <summary>
        /// Set DB migration single command timeout in seconds, default 120 seconds.
        /// </summary>
        /// <param name="commandTimeout">Timeout in seconds.</param>
        /// <returns>Self</returns>
        IMigratorConfiguration SetCommandTimeout(int commandTimeout);

        /// <summary>
        /// Name of schema version metadata table, default 'Schema_Version'.
        /// </summary>
        string SchemaVersionTable { get; }

        /// <summary>
        /// Set name of schema version metadata table, default 'Schema_Version'.
        /// </summary>
        /// <param name="schemaVersionTable">Schema version metadata table name.</param>
        /// <returns>Self</returns>
        IMigratorConfiguration SetSchemaVersionTable(string schemaVersionTable);

        /// <summary>
        /// If to halt DB migration on validation errors, default false.
        /// </summary>
        bool HaltOnValidationError { get; }

        /// <summary>
        /// Set if to halt DB migration on validation errors, default false.
        /// </summary>
        /// <param name="haltOnValidationError"></param>
        /// <returns>Self</returns>
        IMigratorConfiguration SetHaltOnValidationError(bool haltOnValidationError);

        /// <summary>
        /// List of script locations.
        /// </summary>
        IList<ItemLocation> ScriptsLocations { get; }

        /// <summary>
        /// Add additional scripts location.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        /// <param name="scriptsLocation">Scripts location in assembly.</param>
        /// <returns>Self</returns>
        IMigratorConfiguration AddScriptsLocation(Assembly assembly, string scriptsLocation);

        /// <summary>
        /// Migration scripts parameters map.
        /// </summary>
        IDictionary<string, object> Parameters { get; }

        /// <summary>
        /// Add migration scripts parameter.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Self</returns>
        IMigratorConfiguration AddParameter(string key, object value);
    }
}