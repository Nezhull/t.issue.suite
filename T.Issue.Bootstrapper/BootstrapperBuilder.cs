using T.Issue.Bootstrapper.Impl;
using T.Issue.Bootstrapper.Model;

namespace T.Issue.Bootstrapper
{
    public static class BootstrapperBuilder
    {
        public static IBootstrapper<TApplied, TClasspath> Build<TApplied, TClasspath>() where TApplied : AppliedItem
            where TClasspath : ClasspathItem => new BootstrapperImpl<TApplied, TClasspath>();
    }
}
