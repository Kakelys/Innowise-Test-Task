using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using client.Extensions;
using client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace client.Controllers
{
    [Route("/")]
    public class AuthController : BaseController
    {
        public AuthController(ILogger<BaseController> logger, HttpClient client) : base(logger, client)
        {
        }

        [Route("register")]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [Route("login")]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [Route("logout")]
        [HttpGet]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("Token");
            return Redirect("~/");
        }

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> Register(AuthModel model)
        {
            if(!ModelState.IsValid)
                return View();
            
            var response = await client.SendAsync(HttpClientExtension.Methods.POST,"account/register", model);
            if(response.StatusCode != System.Net.HttpStatusCode.OK)
                return Redirect("~/error/500");
            
            var user = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());

            Response.Cookies.Append("Token", user.Token);

            return Redirect("~/");
        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login(AuthModel model)
        {
            if(!ModelState.IsValid)
                return View();

            var response = await client.SendAsync(HttpClientExtension.Methods.POST, "account/login", model);
            if(response.StatusCode != System.Net.HttpStatusCode.OK)
                return Redirect("~/erorr/500");

            var user = JsonConvert.DeserializeObject<UserModel>(await response.Content.ReadAsStringAsync());

            Response.Cookies.Append("Token", user.Token);

            return Redirect("~/");
        }
    }
}