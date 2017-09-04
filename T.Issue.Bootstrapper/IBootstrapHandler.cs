using System.Linq;
using T.Issue.Bootstrapper.Model;

namespace T.Issue.Bootstrapper
{
    public interface IBootstrapHandler<TPending> where TPending : PendingItem
    {
        /// <summary>
        /// Should perform actual pending items bootstrapping.
        /// </summary>
        /// <param name="pendingItems">Pending classpath items.</param>
        void Bootstrap(IOrderedEnumerable<TPending> pendingItems);
    }
}
