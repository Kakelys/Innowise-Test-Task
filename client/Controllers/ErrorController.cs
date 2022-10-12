using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace client.Controllers
{
    [Route("errors")]
    public class ErrorController : BaseController
    {
        public ErrorController(ILogger<BaseController> logger, HttpClient client) : base(logger, client)
        {
        }

        [Route("404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("500")]
        public IActionResult Internal()
        {
            return View();
        }
        
        [Route("403")]
        [Route("401")]
        public new IActionResult Unauthorized()
        {
            return View();
        }
    }
}