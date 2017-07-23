using System.Collections.Generic;
using System.Linq;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;

namespace T.Issue.Bootstrapper.Impl
{
    internal class BootstrapperImpl<TApplied, TClasspath> : IBootstrapper<TApplied, TClasspath> where TApplied : AppliedItem where TClasspath : ClasspathItem
    {
        public bool Bootstrap(IBootstrapHandler<TApplied, TClasspath> handler)
        {
            Assert.NotNull(handler);

            IList<TApplied> appliedItems = handler.GetAppliedItems();
            IList<TClasspath> classpathItems = CollectClasspathItems(handler);

            handler.PreprocessItems(appliedItems, classpathItems);

            if (!ValidateAppliedItems(appliedItems, classpathItems, handler))
            {
                if (!handler.OnValidationErrors())
                {
                    return false;
                }
            }

            var pendingItems = GetPendingItems(appliedItems, classpathItems, handler);

            if (pendingItems.Any())
            {
                handler.BootstrapPendingItems(pendingItems);
            }

            return true;
        }

        private IList<TClasspath> CollectClasspathItems(IBootstrapHandler<TApplied, TClasspath> handler)
        {
            List<TClasspath> result = new List<TClasspath>();

            foreach (var location in handler.GetItemLocations())
            {
                result.AddRange(CollectClasspathItems(location, handler));
            }

            return result;
        }

        private IList<TClasspath> CollectClasspathItems(ItemLocation location, IBootstrapHandler<TApplied, TClasspath> handler)
        {
            var result = new List<TClasspath>();
            var prefix = location.Assembly.GetName().Name + "." + location.Location + ".";

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

                    TClasspath item = handler.BuildClasspathItem(location, itemName, stream);

                    if (item != null)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private IOrderedEnumerable<TClasspath> GetPendingItems(IList<TApplied> appliedItems, IList<TClasspath> classpathItems, IBootstrapHandler<TApplied, TClasspath> handler)
        {
            IList<TClasspath> itemsToRun = new List<TClasspath>();

            foreach (var classpathItem in classpathItems)
            {
                TApplied appliedItem = appliedItems.FirstOrDefault(s => Equals(classpathItem.Id, s.Id) && Equals(classpathItem.Version, s.Version));

                if ((appliedItem == null || (appliedItem.Type == ItemType.Repeatable && !Equals(appliedItem.Checksum, classpathItem.Checksum)))
                    && handler.IsItemPending(appliedItem, classpathItem))
                {
                    itemsToRun.Add(classpathItem);
                }
            }

            return itemsToRun.OrderBy(s => s.Version);
        } 

        private bool ValidateAppliedItems(IList<TApplied> appliedItems, IList<TClasspath> classpathItems, IBootstrapHandler<TApplied, TClasspath> handler)
        {
            bool foundErrors = false;

            foreach (var appliedItem in appliedItems)
            {
                TClasspath classpathItem = classpathItems.FirstOrDefault(s => Equals(appliedItem.Version, s.Version));
                if (classpathItem == null)
                {
                    foundErrors = true;
                    handler.OnValidationError(ErrorType.MissingAppliedClasspathItem, appliedItem, null);
                }
                else if (appliedItem.Type == ItemType.Versioned && !Equals(appliedItem.Checksum, classpathItem.Checksum))
                {
                    foundErrors = true;
                    handler.OnValidationError(ErrorType.AppliedAndClasspathItemChecksumMismatch, appliedItem, classpathItem);
                }
            }

            return !foundErrors;
        }
    }
}