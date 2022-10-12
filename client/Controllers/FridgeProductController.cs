using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using client.Extensions;
using client.Models.FridgeModels;
using client.Models.FridgeProductModels;
using client.Models.ProductModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace client.Controllers
{
    [Route("fridges/{fridgeId:int}/products")]
    public class FridgeProductController : BaseController
    {
        [FromRoute(Name = "fridgeId")]
        public int FridgeId { get; set; }
        public FridgeProductController(ILogger<BaseController> logger, HttpClient client) : base(logger, client)
        {
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var products = await client.GetDataAsync<List<Product>>("products");
            AddFridgeProduct model = products == null ? null :
                new AddFridgeProduct(){Products = products};

            return CheckGetModel(model);
        }

        [Route("{productId}/edit")]
        [HttpGet]
        public async Task<IActionResult> ProductEdit(int productId)
        {
            var product = await client.GetDataAsync<FridgeProduct>($"fridges/{FridgeId}/products/{productId}");

            EditFridgeProduct model = product == null ? null :
                new(){Product = product};

            return CheckGetModel(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddFridgeProduct product)
        {
            if(!ModelState.IsValid)
            {
                product.Products = JsonConvert.DeserializeObject<List<Product>>(Request.Form["products"].ToString());

                return View(product);
            } 

            await client.SendAsync(HttpClientExtension.Methods.POST, $"fridges/{FridgeId}/products", product);

            return Redirect("detail");
        }

        [Route("{productId}/edit")]
        [HttpPost]
        public async Task<IActionResult> ProductEdit(int productId, EditFridgeProduct model)
        {
            if(!ModelState.IsValid)
            {
                model.Product = JsonConvert.DeserializeObject<FridgeProduct>(Request.Form["Product"].ToString());

                return View(model);
            }

            await client.SendAsync(HttpClientExtension.Methods.PUT, $"fridges/{FridgeId}/products/{productId}", model);

            return RedirectPermanent("../../detail");
        }

        [Route("{productId}/take")]
        [HttpPost]
        public async Task<IActionResult> Detail(int productId, FridgeDetail model)
        {
            model.ProductId = productId;
            if(!ModelState.IsValid)
            {
                model.FridgeInDetail = JsonConvert.DeserializeObject<Fridge>(Request.Form["FridgeInDetail"].ToString());

                return View(model);
            }
            
            await client.SendAsync(HttpClientExtension.Methods.POST, $"fridges/{FridgeId}/products/{productId}/take", model);

            return RedirectPermanent("../../detail");
        }
    }
}