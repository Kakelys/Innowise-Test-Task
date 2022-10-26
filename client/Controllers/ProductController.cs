using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using client.Extensions;
using client.Models;
using client.Models.ProductModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace client.Controllers
{
    [Route("products")]
    public class ProductController : BaseController
    {
        public ProductController(ILogger<BaseController> logger, HttpClient client) : base(logger, client)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await client.GetDataAsync<List<Product>>("products");

            return View(products);
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("{id}/edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await client.GetDataAsync<Product>($"products/{id}");

            ProductEditModel model = product == null ? null : 
                new(){Product = product};

            return CheckGetModel(model);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            using var fileStream = model.Picture.OpenReadStream();

            byte[] bytes = new byte[model.Picture.Length];
            fileStream.Read(bytes, 0, (int)model.Picture.Length);

            var content = new 
            {
                Name = model.Name,
                DefaultQuantity = model.DefaultQuantity,
                Picture = bytes,
                PictureExntension = Path.GetExtension(model.Picture.FileName)
            };

            await client.SendAsync(HttpClientExtension.Methods.POST, "products", content);

            return RedirectPermanent("~/products");
        }

        [Route("{id}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductEditModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Product = JsonConvert.DeserializeObject<Product>(Request.Form["Product"].ToString());

                return View(model);
            }

            if(model.Picture == null)
            {
                var content = new 
                {
                    Name = model.Name,
                    DefaultQuantity = model.DefaultQuantity,
                };

                await client.SendAsync(HttpClientExtension.Methods.PUT, $"products/{id}", content);
            }
            else
            {
                using var fileStream = model.Picture.OpenReadStream();

                byte[] bytes = new byte[model.Picture.Length];
                fileStream.Read(bytes, 0, (int)model.Picture.Length);

                var content = new 
                {
                    Name = model.Name,
                    DefaultQuantity = model.DefaultQuantity,
                    Picture = bytes,
                    PictureExntension = Path.GetExtension(model.Picture.FileName)
                };
                
                await client.SendAsync(HttpClientExtension.Methods.PUT, $"products/{id}", content);
            }

            return RedirectPermanent("~/products");
        }

        [Route("{id}/delete")]
        [HttpPost]
        public async Task<RedirectResult> Delete(int id)
        {
            await client.SendAsync(HttpClientExtension.Methods.DELETE, $"products/{id}");

            return RedirectPermanent("~/products");
        }
    }
}