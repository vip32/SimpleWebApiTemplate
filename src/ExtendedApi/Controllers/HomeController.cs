using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace ExtendedApi
{
    [Route("")]
    public class HomeController : ApiController
    {
        private readonly IMessageProvider _messageProvider;

        public HomeController(IMessageProvider messageProvider)
        {
            if(messageProvider == null) throw new ArgumentException(nameof(messageProvider));

            _messageProvider = messageProvider;
        }

        public IHttpActionResult Get()
        {
            return new ResponseMessageResult(new HttpResponseMessage
            {
                Content = new StringContent(_messageProvider.WelcomeText)
            });
        }
    }
}
