using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/fridges")]
    public class FridgeController : BaseApiController
    {
        public FridgeController(IRepositoryManager repository) : base(repository)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fridge>>> Get()
        {
            return Ok(await _repository.Fridge.GetAllFridgesAsync(false));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fridge>> GetById(int? id)
        {
            var fridge = await _repository.Fridge.GetByIdAsync(id, false);

            if(fridge == null)
                return NotFound(ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            return Ok(fridge);
        }

        [HttpGet("{id}/detail")]
        public async Task<ActionResult<Fridge>> GetFridgeDetail(int id)
        {
            var fridge = await _repository.Fridge.GetFridgeDetail(id, false);

            if(fridge == null)
                return NotFound(ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            return Ok(fridge);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Fridge> PostFridge(CreateFridgeDto fridge)
        {
            var fr = new Fridge()
            {
                Name = fridge.Name,
                OwnerName = fridge.OwnerName,
                Model = fridge.Model,
            };

            try
            {
                _repository.BeginTransaction();

                _repository.Fridge.Create(fr);
                _repository.Save();

                if(fridge.Products != null)
                {
                    var fridgeProducts = new List<FridgeProduct>(
                        fridge.Products.Select(x=>new FridgeProduct()
                        {
                            Product = x.Id, Fridge = fr.Id, Quantity = x.DefaultQuantity
                        }
                        ));
                    fr.FridgeProducts = fridgeProducts;
                }

                _repository.Save();
                _repository.Commit();
            }
            catch
            {
                _repository.Rollback();
                return BadRequest("Something went wrong when building a new refrigerator");
            }
            
            return Ok();
        }

        [Authorize]
        [HttpPost("updproducts")]
        public async Task<ActionResult<bool>> UpdProducts()
        {
            var list = new List<FridgeProduct>(await _repository.FridgeProduct.FindEmptyAsync());
            if(list.Count == 0)
                return true;

            using var client = new HttpClient();
            var product = new FridgeProductDto();
            
            for(var i = 0; i < list.Count; i++)
            {
                product.ProductId = (int)list[i].Product;
                product.Quantity = list[i].Quantity;

                await client.PostAsync($"https://localhost:5001/api/fridges/{list[i].Fridge}/products", 
                    new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
            }

            return Ok();
        }

        [Authorize(Policy = "ForAdmins")]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> PutFridge(int id, EditFridgeDto fridge)
        {
            var fr = await _repository.Fridge.GetByIdAsync(id, true);

            if(fr == null)
                 return NotFound(ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            fr.Name = fridge.Name;
            fr.OwnerName = fridge.OwnerName;
            fr.Model = fridge.Model;

            _repository.Save();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteFridge(int id)
        {
            var fridge = await _repository.Fridge.GetByIdAsync(id, true);

            if(fridge == null)
                return NotFound(ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            _repository.Fridge.Delete(fridge);
                
            _repository.Save();
            return Ok();
        }
    }
}