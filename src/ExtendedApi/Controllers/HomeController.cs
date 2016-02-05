using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace ExtendedApi.Controllers
{
    public class HomeController : ApiController
    {
        public IHttpActionResult Get()
        {
            return new ResponseMessageResult(new HttpResponseMessage
            {
                Content = new StringContent("Welcome to my WebApi")
            });
        }
    }
}
