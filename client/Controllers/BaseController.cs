using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace client.Controllers
{
    public class BaseController : Controller
    {
        public LayoutModel LayoutModel { get; set; }
        protected readonly ILogger<BaseController> logger;
        protected readonly HttpClient client;

        private bool _isHeadersSet;

        public BaseController(ILogger<BaseController> logger, HttpClient client)
        {
            this.logger = logger;
            this.client = client;
            _isHeadersSet = false;

            LayoutModel = new();
            ViewData["layout"] = LayoutModel;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = HttpContext.Request.Cookies["Token"];
            if(string.IsNullOrEmpty(token))
            {
                base.OnActionExecuting(context);
                return;
            }
            
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            
            var roleClaim = securityToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            var user = new User()
            {
                UserName = securityToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value,
                Role = string.IsNullOrEmpty(roleClaim) ? 0 : int.Parse(roleClaim)
            };

            ViewData["layout"] = new LayoutModel(user);

            base.OnActionExecuting(context);
        }
        protected IActionResult CheckGetModel<T>(T model)
        {
            if(model == null || model.Equals(default(T)))
                return Redirect("~/error/404");

            return View(model);
        }
    }
}