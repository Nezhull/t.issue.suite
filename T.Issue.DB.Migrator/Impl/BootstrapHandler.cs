using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.FileSystems;
using DotLiquid.NamingConventions;
using T.Issue.Bootstrapper;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;
using log4net;
using T.Issue.DB.Migrator.Utils;

namespace T.Issue.DB.Migrator.Impl
{
    internal class BootstrapHandler : IBootstrapHandler<AppliedItem, ClasspathItem>
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(DatabaseMigratorImpl));

        private readonly Regex scriptNameRegex = new Regex(@"^(V|R)(\d{4}\.\d{2}\.\d{2}_\d{4})_(.+)\.sql$");

        private readonly IMigratorConfiguration configuration;
        private readonly IDbAccessFacade dbAccessFacade;
        private readonly SqlConnection connection;

        private Hash templateContext;

        public BootstrapHandler(SqlConnection connection, IMigratorConfiguration configuration, IDbAccessFacade dbAccessFacade)
        {
            Assert.NotNull(connection);
            Assert.NotNull(configuration);
            Assert.NotNull(dbAccessFacade);

            this.configuration = configuration;
            this.dbAccessFacade = dbAccessFacade;
            this.connection = connection;
        }

        public void Init()
        {
            Assert.IsNotEmpty(configuration.ScriptsLocations);
            Assert.HasText(configuration.SchemaVersionTable);

            Template.RegisterFilter(typeof(BinaryToHexFilter));
            Template.NamingConvention = new CSharpNamingConvention();
            templateContext = Hash.FromDictionary(configuration.Parameters);

            LOG.Info("Starting DB migration from locations:");
            foreach (var location in configuration.ScriptsLocations)
            {
                LOG.InfoFormat("Location: {0}, assembly: {1}", location.Location, location.Assembly.GetName());
            }

            dbAccessFacade.CreateSchemaTableIfNotExist(connection, configuration);
        }

        public IList<AppliedItem> GetAppliedItems()
        {
            return dbAccessFacade.GetAppliedScripts(connection, configuration);
        }

        public IList<ItemLocation> GetItemLocations()
        {
            return configuration.ScriptsLocations;
        }

        public ClasspathItem BuildClasspathItem(ItemLocation location, string itemName, Stream content)
        {
            Match matchResult = scriptNameRegex.Match(itemName);

            if (!matchResult.Success)
            {
                LOG.WarnFormat("File name {0} does not match migration script naming convention and will be ignored.", itemName);
                return null;
            }

            ItemType type = ScriptTypeUtils.ResolveType(matchResult.Groups[1].Value);
            string version = string.Join(string.Empty, matchResult.Groups[2].Value.Split('.', '_'));

            using (var reader = new StreamReader(content, Encoding.UTF8))
            {
                string template = reader.ReadToEnd();

                var item = new ClasspathItem
                {
                    Id = version,
                    Type = type,
                    Version = version,
                    Name = itemName,
                    Content = PreprocessScriptTemplate(location, template),
                    Checksum = StringUtils.ByteArrayToHex(HashUtils.GetMD5Hash(Encoding.UTF8.GetBytes(template)))
                };

                LOG.DebugFormat("Adding file {0} as update script with version {1}", itemName, version);

                return item;
            }
        }

        public void PreprocessItems(IList<AppliedItem> appliedItems, IList<ClasspathItem> classpathItems)
        {
            string currentVersion = appliedItems.Where(i => i.Type == ItemType.Versioned).Select(i => i.Version).Max(i => i);
            string repeatableVersion = appliedItems.Where(i => i.Type == ItemType.Repeatable).Select(i => i.Version).Max(i => i);

            if (currentVersion != null)
            {
                string version = string.Format("Current schema version: {0}", currentVersion);

                if (repeatableVersion != null)
                {
                    version = string.Format("{0} ({1})", version, repeatableVersion);
                }

                LOG.InfoFormat(version);
            }
        }

        public bool IsItemPending(AppliedItem appliedItem, ClasspathItem classpathItem)
        {
            return true;
        }

        public void BootstrapPendingItems(IOrderedEnumerable<ClasspathItem> pendingItems)
        {
            dbAccessFacade.ApplyPendingScripts(connection, configuration, pendingItems);
        }

        public void OnValidationError(ErrorType error, AppliedItem appliedItem, ClasspathItem classpathItem)
        {
            switch (error)
            {
                case ErrorType.MissingAppliedClasspathItem:
                    LOG.ErrorFormat("Unable to find schema migration script {0} hash {1} applied at {2}!", appliedItem.Name, appliedItem.Checksum, appliedItem.Executed);
                    break;

                case ErrorType.AppliedAndClasspathItemChecksumMismatch:
                    LOG.ErrorFormat("Applied versioned migration script {0} hash {1} is different from file hash {2}!", appliedItem.Name, appliedItem.Checksum, classpathItem.Checksum);
                    break;
                default:
                    LOG.ErrorFormat("Validation error for script {0}", appliedItem.Name);
                    break;
            }
        }

        public bool OnValidationErrors()
        {
            if (configuration.HaltOnValidationError)
            {
                LOG.Info("Migration errors found! Stopping DB migration!");
                return false;
            }
            return true;
        }

        private string PreprocessScriptTemplate(ItemLocation location, string scriptTemplate)
        {
            Template template = Template.Parse(scriptTemplate);
            templateContext[BinaryToHexFilter.CurrentLocation] = location;
            return template.Render(templateContext);
        }
    }
}
