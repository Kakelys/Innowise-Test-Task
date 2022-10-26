using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Interfaces;
using API.Interfaces.Logics;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/fridges/{fridgeId:int}/products")]
    public class FridgeProductController : BaseApiController
    {
        [FromRoute(Name = "fridgeId")]
        public int FridgeId { get; set; }
        private readonly IFridgeProductService _fridgeProductService;
        public FridgeProductController(IFridgeProductService fridgeProductService)
        {
            _fridgeProductService = fridgeProductService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FridgeProduct>>> GetProducts()
        {
            return Ok(await _fridgeProductService.GetProducts(FridgeId, false));
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<FridgeProduct>> GetProduct(int productId)
        {
            return Ok(await _fridgeProductService.GetProduct(FridgeId, productId, false));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddProduct(FridgeProductDto product)
        {
            return Ok(await _fridgeProductService.Add(FridgeId, product));
        }

        [HttpPost("{productId}/take")]
        public async Task<ActionResult<bool>> TakeProduct(int productId, FridgeProductDto product)
        {
            return Ok(await _fridgeProductService.Take(productId, product));
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<bool>> UpdProduct(int productId, FridgeProductDto product)
        {
            return Ok(await _fridgeProductService.Update(productId, product));
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<bool>> DelProduct(int productId)
        {
            return Ok(await _fridgeProductService.Delete(productId));
        }
    }
}