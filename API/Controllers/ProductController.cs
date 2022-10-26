using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Interfaces;
using API.Interfaces.Logics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/products")]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return Ok(await _productService.GetProducts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return Ok(await _productService.GetProductById(id, false));
        }

        [HttpPost]
        public ActionResult AddProduct(ProductDto product)
        {
            return Ok(_productService.Add(product));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductEditDto product)
        {
            return Ok(await _productService.Update(id, product));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DelProduct(int id)
        {
            return Ok(await _productService.Delete(id));
        }
    }
}