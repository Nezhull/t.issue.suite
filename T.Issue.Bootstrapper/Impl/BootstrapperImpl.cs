using System;
using System.Collections.Generic;
using System.Linq;
using T.Issue.Bootstrapper.Collectors;
using T.Issue.Bootstrapper.Model;
using T.Issue.Commons.Utils;

namespace T.Issue.Bootstrapper.Impl
{
    public class BootstrapperImpl<TApplied, TPending> : IBootstrapper<TApplied, TPending> where TApplied : AppliedItem where TPending : PendingItem
    {
        private readonly IBootstrapHandler<TPending> handler;
        private readonly IBootstrapItemCollector<TPending> pendingCollector;
        private readonly IBootstrapItemCollector<TApplied> appliedCollector;

        public bool HaltOnValidationError { get; set; }
        
        public Action<ValidationContext<TApplied, TPending>> OnValidationError { protected get; set; }
        public Action<IList<TPending>, IList<TApplied>> OnItemsCollected { protected get; set; }
        public Func<TApplied, TPending, bool> OnAddItemToApply { protected get; set; }

        public BootstrapperImpl(IBootstrapHandler<TPending> handler, IBootstrapItemCollector<TPending> pendingCollector, IBootstrapItemCollector<TApplied> appliedCollector)
        {
            Assert.NotNull(handler);
            Assert.NotNull(pendingCollector);
            Assert.NotNull(appliedCollector);
            
            this.handler = handler;
            this.pendingCollector = pendingCollector;
            this.appliedCollector = appliedCollector;
        }

        public virtual bool Bootstrap()
        {
            IList<TPending> pendingItems = pendingCollector.Collect();
            IList<TApplied> appliedItems = appliedCollector.Collect();

            OnItemsCollected?.Invoke(pendingItems, appliedItems);

            if (!Validate(appliedItems, pendingItems))
            {
                if (HaltOnValidationError)
                {
                    return false;
                }
            }

            IOrderedEnumerable<TPending> toApply = CollectToApply(appliedItems, pendingItems);

            if (toApply.Any())
            {
                handler.Bootstrap(toApply);
            }

            return true;
        }

        private IOrderedEnumerable<TPending> CollectToApply(IList<TApplied> appliedItems, IList<TPending> pendingItems)
        {
            IList<TPending> itemsToRun = new List<TPending>();

            foreach (var pendingItem in pendingItems)
            {
                TApplied appliedItem = appliedItems.FirstOrDefault(s => Equals(pendingItem.Id, s.Id) && Equals(pendingItem.Version, s.Version));

                if (appliedItem != null && (appliedItem.Type != ItemType.Repeatable || Equals(appliedItem.Checksum, pendingItem.Checksum)))
                {
                    continue;
                }
                
                if (OnAddItemToApply?.Invoke(appliedItem, pendingItem) ?? true)
                {
                    itemsToRun.Add(pendingItem);
                }
            }

            return itemsToRun.OrderBy(s => s.Version);
        } 

        protected virtual bool Validate(IList<TApplied> appliedItems, IList<TPending> pendingItems)
        {
            bool foundErrors = false;

            foreach (var appliedItem in appliedItems)
            {
                TPending pendingItem = pendingItems.FirstOrDefault(s => Equals(appliedItem.Version, s.Version));
                if (pendingItem == null)
                {
                    foundErrors = true;
                    OnValidationError?.Invoke(new ValidationContext<TApplied, TPending>(ErrorType.MissingAppliedClasspathItem, null, appliedItem));
                }
                else if (appliedItem.Type == ItemType.Versioned && !Equals(appliedItem.Checksum, pendingItem.Checksum))
                {
                    foundErrors = true;
                    OnValidationError?.Invoke(new ValidationContext<TApplied, TPending>(ErrorType.AppliedAndClasspathItemChecksumMismatch, pendingItem, appliedItem));
                }
            }

            return !foundErrors;
        }
    }
}