using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Farm.Controllers
{
    [Route("api/farm")]
    [ApiController]
    public class FarmController : ControllerBase
    {
        // GET api/values
        [HttpGet, Route("")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Farm1", "Farm2" };
        }
    }
}
