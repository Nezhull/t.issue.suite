using System.Collections.Generic;
using System.IO;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;

namespace T.Issue.Bootstrapper.Collectors
{
    public abstract class ClasspathItemCollectorBase : IBootstrapItemCollector<PendingItem>
    {
        private readonly IList<ItemLocation> itemLocations;

        protected ClasspathItemCollectorBase(IList<ItemLocation> itemLocations)
        {
            Assert.IsNotEmpty(itemLocations);
            
            this.itemLocations = itemLocations;
        }

        public virtual IList<PendingItem> Collect()
        {
            List<PendingItem> result = new List<PendingItem>();

            foreach (var location in itemLocations)
            {
                result.AddRange(CollectClasspathItems(location));
            }

            return result;
        }
        
        protected virtual IList<PendingItem> CollectClasspathItems(ItemLocation location)
        {
            List<PendingItem> result = new List<PendingItem>();
            string prefix = location.Assembly.GetName().Name + "." + location.Location + ".";

            foreach (var resource in location.Assembly.GetManifestResourceNames())
            {
                if (!resource.StartsWith(prefix))
                {
                    continue;
                }

                string itemName = resource.Substring(prefix.Length);

                using (var stream = location.Assembly.GetManifestResourceStream(resource))
                {
                    Assert.NotNull(stream);

                    PendingItem item = BuildClasspathItem(location, itemName, stream);

                    if (item != null)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Should build classpath item.
        /// </summary>
        /// <param name="location">Item location.</param>
        /// <param name="itemName">Item name.</param>
        /// <param name="content">Content stream.</param>
        /// <returns>Classpath item</returns>
        protected abstract PendingItem BuildClasspathItem(ItemLocation location, string itemName, Stream content);
    }
}