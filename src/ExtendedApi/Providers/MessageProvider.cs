using System.Threading;
using Serilog;
using Serilog.Events;

namespace ExtendedApi
{
    class MessageProvider : IMessageProvider
    {
        private readonly ILogger _contextLogger;

        public MessageProvider()
        {
            _contextLogger = Log.ForContext<MessageProvider>();
            _contextLogger.Debug("ctor");
        }

        public string WelcomeText
        {
            get
            {
                using (_contextLogger.BeginTimedOperation("Timed operation", level: LogEventLevel.Debug))
                {
                    Thread.Sleep(500);
                    return "Welcome to the Extended WebApi";
                }
            }
        }
    }
}