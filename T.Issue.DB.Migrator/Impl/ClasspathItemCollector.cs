using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.NamingConventions;
using log4net;
using T.Issue.Bootstrapper.Collectors;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;
using T.Issue.DB.Migrator.Utils;

namespace T.Issue.DB.Migrator.Impl
{
    public class ClasspathItemCollector : ClasspathItemCollectorBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClasspathItemCollector));
        private static readonly Regex ScriptNameRegex = new Regex(@"^(V|R)(\d{4}\.\d{2}\.\d{2}_\d{4})_(.+)\.sql$");
        
        private readonly Hash templateContext;
        
        public ClasspathItemCollector(IList<ItemLocation> itemLocations, IDictionary<string, object> templateContext) : base(itemLocations)
        {
            this.templateContext = Hash.FromDictionary(templateContext);
        }

        protected override PendingItem BuildClasspathItem(ItemLocation location, string itemName, Stream content)
        {
            Match matchResult = ScriptNameRegex.Match(itemName);

            if (!matchResult.Success)
            {
                Log.WarnFormat("File name {0} does not match migration script naming convention and will be ignored.", itemName);
                return null;
            }

            ItemType type = ScriptTypeUtils.ResolveType(matchResult.Groups[1].Value);
            string version = string.Join(string.Empty, matchResult.Groups[2].Value.Split('.', '_'));

            using (var reader = new StreamReader(content, Encoding.UTF8))
            {
                string template = reader.ReadToEnd();

                var item = new PendingItem
                {
                    Id = version,
                    Type = type,
                    Version = version,
                    Name = itemName,
                    Content = PreprocessScriptTemplate(location, template),
                    Checksum = StringUtils.ByteArrayToHex(HashUtils.GetMD5Hash(Encoding.UTF8.GetBytes(template)))
                };

                Log.DebugFormat("Adding file {0} as update script with version {1}", itemName, version);

                return item;
            }
        }
        
        private string PreprocessScriptTemplate(ItemLocation location, string scriptTemplate)
        {
            Template template = Template.Parse(scriptTemplate);
            templateContext[BinaryToHexFilter.CurrentLocation] = location;
            return template.Render(templateContext);
        }
    }
}