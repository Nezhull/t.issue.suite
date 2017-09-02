namespace T.Issue.Bootstrapper.Model
{
    public class ValidationContext<TApplied, TPending> where TApplied : AppliedItem where TPending : PendingItem
    {
        public ErrorType ErrorType { get; }
        public TPending PendingItem { get; }
        public TApplied AppliedItem { get; }
        
        public ValidationContext(ErrorType errorType, TPending pendingItem, TApplied appliedItem)
        {
            ErrorType = errorType;
            PendingItem = pendingItem;
            AppliedItem = appliedItem;
        }
    }
}