using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Api.Farm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET api/values
        [HttpGet, Route("")]
        public ActionResult<IEnumerable<string>> Index()
        {
            return new string[] { "Farm1", "Farm2" };
        }
    }
}
