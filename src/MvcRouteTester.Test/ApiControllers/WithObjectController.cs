using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
    public class InputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class WithObjectController : ApiController
    {
        public HttpResponseMessage Get(InputModel data)
        {
            return new HttpResponseMessage();
        }

        public Task<HttpResponseMessage> Get2(InputModel data)
        {
            return Task.FromResult<HttpResponseMessage>(null);
        }

        public IHttpActionResult Get3(InputModel data)
        {
            return Ok();
        }

        public Task<IHttpActionResult> Get4(InputModel data)
        {
            return Task.FromResult<IHttpActionResult>(null);
        }
    }
}
