using System;
using System.Collections.Generic;
using T.Issue.Bootstrapper.Model;

namespace T.Issue.Bootstrapper
{
    public interface IBootstrapper<TApplied, TPending> where TApplied : AppliedItem where TPending : PendingItem
    {
        Action<ValidationContext<TApplied, TPending>> OnValidationError { set; }
        Action<IList<TPending>, IList<TApplied>> OnItemsCollected { set; }
        Func<TApplied, TPending, bool> OnAddItemToApply { set; }
        
        bool HaltOnValidationError { get; set; }
        
        bool Bootstrap();
    }
}