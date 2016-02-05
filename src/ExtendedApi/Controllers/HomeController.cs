using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Serilog;

namespace ExtendedApi
{
    [Route("")]
    public class HomeController : ApiController
    {
        private readonly IMessageProvider _messageProvider;
        private readonly ILogger _contextLogger;

        public HomeController(IMessageProvider messageProvider)
        {
            if(messageProvider == null) throw new ArgumentException(nameof(messageProvider));

            _contextLogger = Log.ForContext<HomeController>();
            _messageProvider = messageProvider;

            _contextLogger.Debug("ctor");
        }

        public IHttpActionResult Get()
        {
            _contextLogger.Debug("response:{message}", _messageProvider.WelcomeText);

            return new ResponseMessageResult(new HttpResponseMessage
            {
                Content = new StringContent(_messageProvider.WelcomeText)
            });
        }
    }
}
