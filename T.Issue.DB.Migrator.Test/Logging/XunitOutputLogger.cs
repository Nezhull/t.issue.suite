using System;
using Common.Logging;
using Common.Logging.Factory;
using Xunit.Abstractions;

namespace T.Issue.DB.Migrator.Test.Logging
{
    public class XunitOutputLogger : AbstractLogger
    {
        private readonly LogLevel useLogLevel;
        private readonly ITestOutputHelper output;

        public XunitOutputLogger(LogLevel useLogLevel, ITestOutputHelper output)
        {
            this.useLogLevel = useLogLevel;
            this.output = output;
        }

        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            if (exception != null)
            {
                output.WriteLine(level + ": " + message + exception);
            }
            else
            {
                output.WriteLine(level + ": " + message);
            }
        }

        public override bool IsTraceEnabled => useLogLevel <= LogLevel.Trace;

        public override bool IsDebugEnabled => useLogLevel <= LogLevel.Debug;

        public override bool IsInfoEnabled => useLogLevel <= LogLevel.Info;

        public override bool IsWarnEnabled => useLogLevel <= LogLevel.Warn;

        public override bool IsErrorEnabled => useLogLevel <= LogLevel.Error;

        public override bool IsFatalEnabled => useLogLevel <= LogLevel.Fatal;
    }
}
