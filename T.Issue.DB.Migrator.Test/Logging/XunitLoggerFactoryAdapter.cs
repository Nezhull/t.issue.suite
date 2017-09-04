using Common.Logging;
using Common.Logging.Factory;
using Xunit.Abstractions;

namespace T.Issue.DB.Migrator.Test.Logging
{
    public class XunitLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        private readonly LogLevel useLogLevel;
        private readonly ITestOutputHelper output;

        public XunitLoggerFactoryAdapter(LogLevel useLogLevel, ITestOutputHelper output) : base(false)
        {
            this.useLogLevel = useLogLevel;
            this.output = output;
        }

        protected override ILog CreateLogger(string name)
        {
            return new XunitOutputLogger(useLogLevel, output);
        }
    }
}

