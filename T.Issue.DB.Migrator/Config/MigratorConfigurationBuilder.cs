using System.Collections.Generic;
using System.Reflection;
using T.Issue.Bootstrapper.Model;

namespace T.Issue.DB.Migrator.Config
{
    public class MigratorConfigurationBuilder
    {
        public static IMigratorConfiguration Build(Assembly assembly) => new MigratorConfigurationImpl(assembly);
        public static IMigratorConfiguration Build(Assembly assembly, string scriptsLocation) => new MigratorConfigurationImpl(assembly, scriptsLocation);
        public static IMigratorConfiguration Build(ItemLocation location) => new MigratorConfigurationImpl(location);
        public static IMigratorConfiguration Build(IList<ItemLocation> locations) => new MigratorConfigurationImpl(locations);
    }
}
