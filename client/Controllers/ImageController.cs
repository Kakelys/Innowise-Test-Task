using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using client.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace client.Controllers
{
    public class ImageController : BaseController
    {
        public ImageController(ILogger<BaseController> logger, HttpClient client) : base(logger, client)
        {
        }

        [HttpGet]
        public async Task<System.IO.Stream> GetImage(int id)
        {
            try
            {
                var data = await client.GetDataAsync($"images/{id}");

                return await data.Content.ReadAsStreamAsync();
            }
            catch(HttpRequestException)
            {
                return null;
            }
        }
    }
}