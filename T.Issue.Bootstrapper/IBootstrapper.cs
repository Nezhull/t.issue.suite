using T.Issue.Bootstrapper.Model;

namespace T.Issue.Bootstrapper
{
    public interface IBootstrapper<TApplied, TClasspath> where TApplied : AppliedItem where TClasspath : ClasspathItem
    {
        bool Bootstrap(IBootstrapHandler<TApplied, TClasspath> handler);
    }
}