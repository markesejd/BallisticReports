using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BallisticReports.Controllers
{
    public class DemoController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new[] { "Hello", "World" };
        }

        public string Get(int id)
        {
            return "Hello World!";
        }
    }
}
