using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/fridges/{fridgeId:int}/products")]
    public class FridgeProductController : BaseApiController
    {
        [FromRoute(Name = "fridgeId")]
        public int FridgeId { get; set; }
        public FridgeProductController(IRepositoryManager repository) : base(repository)
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<FridgeProduct>>> GetProducts()
        {
            return Ok(await _repository.FridgeProduct.GetAllByFridgeId(FridgeId,false));
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<FridgeProduct>> GetProduct(int productId)
        {
            if(!(await IsFridgeExist(FridgeId)))
                return NotFound(ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            var product = await _repository.FridgeProduct.GetByIdWithProduct(productId, false);
            
            if(product == null)
                return NotFound(ApiException.GetErrorMessage(ApiException.Errors.ProductNotFound));

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddProduct(FridgeProductDto product)
        {
            if(!(await IsFridgeExist(FridgeId)))
                return BadRequest(ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            var fp = await _repository.FridgeProduct.GetByProductId(product.ProductId, FridgeId, true);

            if(fp != null)
            {
                fp.Quantity += product.Quantity;
            }
            else
            {
                _repository.FridgeProduct.Create(
                    new FridgeProduct()
                    {
                        Product = product.ProductId,
                        Quantity = product.Quantity,
                        Fridge = FridgeId
                    });
            }

            _repository.Save();
            return Ok();
        }

        [HttpPost("{productId}/take")]
        public async Task<ActionResult<bool>> TakeProduct(int productId, FridgeProductDto product)
        {
            var fp = await _repository.FridgeProduct.GetById(productId, true);

            if(fp == null)
                return NotFound(ApiException.GetErrorMessage(ApiException.Errors.ProductNotFound));

            if(fp.Quantity - product.Quantity < 0)
                return BadRequest("Fridge doesn't have enough amount of products");

            fp.Quantity -= product.Quantity;
            if(fp.Quantity == 0)
                _repository.FridgeProduct.Delete(fp);

            _repository.Save();
            return Ok();
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<bool>> UpdProduct(int productId, FridgeProductDto product)
        {
            if(!(await IsFridgeExist(FridgeId)))
                return NotFound(ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            var entity = await _repository.FridgeProduct.GetById(productId, true);

            if(entity == null)
                return NotFound(ApiException.GetErrorMessage(ApiException.Errors.ProductNotFound));

            entity.Quantity = product.Quantity;

            _repository.Save();
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<bool>> DelProduct(int productId)
        {
            if(!(await IsFridgeExist(FridgeId)))
                return NotFound(ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            var fp = await _repository.FridgeProduct.GetById(FridgeId, false);

            if(fp == null)
                return NotFound("There is no product, that matches the request");

            _repository.FridgeProduct.Delete(fp);

            _repository.Save();
            return Ok();
        }
    }
}