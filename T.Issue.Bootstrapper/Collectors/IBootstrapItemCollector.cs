using System.Collections.Generic;
using T.Issue.Bootstrapper.Model;

namespace T.Issue.Bootstrapper.Collectors
{
    public interface IBootstrapItemCollector<T> where T : BootstrapItem
    {
        IList<T> Collect();
    }
}