using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using client.Extensions;
using client.Models;
using client.Models.FridgeModels;
using client.Models.FridgeProductModels;
using client.Models.ProductModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace client.Controllers
{
    [Route("fridges")]
    public class FridgeController : BaseController
    {
        public FridgeController(ILogger<BaseController> logger, HttpClient client) : base(logger, client)
        {   
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fridges = await client.GetDataAsync<List<Fridge>>("fridges");
            
            return View(fridges);
        }

        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var models = await client.GetDataAsync<List<FridgeModel>>("models");
            var list = await client.GetDataAsync<List<Product>>("products");

            //create randomized groups of products
            var groups = new List<List<Product>>();
            var rand = new Random();

            if(list.Count>0)
            {
                for(var i = 0; i < 5; i++)
                {
                    var group = new HashSet<Product>();
                    for(var j =0; j < 3; j++)
                    {
                        group.Add(list[rand.Next(0,list.Count)]);
                    }
                    groups.Add(group.ToList());
                }
            }

            var model = new CreateFridge()
            {
                Models = models,
                ProductGroups = groups
            };

            return CheckGetModel(model);
        }

        [Route("{id}/edit")]
        [HttpGet]        
        public async Task<IActionResult> Edit(int id)
        {
            var fridge = await client.GetDataAsync<Fridge>($"fridges/{id}");
            var models = await client.GetDataAsync<List<FridgeModel>>("models");

            EditFridge model = null;

            if(fridge != null && models != null)
                model = new EditFridge()
                {
                    Fridge = fridge,
                    Models = models
                };

            return CheckGetModel(model);
        }

        [Route("{id}/detail")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var fridge = await client.GetDataAsync<Fridge>($"fridges/{id}/detail");

            FridgeDetail model = fridge == null ? null:
                new(){FridgeInDetail  = fridge};

            return CheckGetModel(model);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateFridge fridge)
        {
            if(!ModelState.IsValid)
            {
                fridge.Models = JsonConvert.DeserializeObject<List<FridgeModel>>(Request.Form["Models"].ToString());
                fridge.ProductGroups = JsonConvert.DeserializeObject<List<List<Product>>>(Request.Form["ProductGroups"].ToString());
                return View(fridge);
            }

            fridge.Products = JsonConvert.DeserializeObject<List<Product>>(Request.Form["Products"].ToString());
            
            await client.SendAsync(HttpClientExtension.Methods.POST, $"fridges", fridge);

            return RedirectToRoute(new {controller="Fridge", Action="Index"});
        }

        [Route("{id}/delete")]
        [HttpPost]
        public async Task<RedirectToRouteResult> Delete(int id)
        {
            await client.SendAsync(HttpClientExtension.Methods.DELETE, $"fridges/{id}");

            return RedirectToRoute(new {controller="Fridge", Action="Index"});
        }

        [HttpPost("updfridgeproducts")]
        public async Task<RedirectResult> updFridgeProducts()
        {
            await client.SendAsync(HttpClientExtension.Methods.POST, "fridges/updproducts", new{});
            
            return RedirectPermanent("../");
        }

        [Route("{id}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditFridge newFridge)
        {
            if(!ModelState.IsValid)
            {
                newFridge.Models = JsonConvert.DeserializeObject<List<FridgeModel>>(Request.Form["Models"].ToString());
                newFridge.Fridge = JsonConvert.DeserializeObject<Fridge>(Request.Form["Fridge"].ToString());
                return View(newFridge);
            }
            
            await client.SendAsync(HttpClientExtension.Methods.PUT, $"fridges/{id}", newFridge);

            return RedirectToRoute(new {controller="Fridge", Action="Index"});
        }        
    }
}