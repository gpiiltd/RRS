using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRS.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [Route("api/irs")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Welcome to IRS API" };
        }
    }
}
