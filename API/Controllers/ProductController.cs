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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/products")]
    public class ProductController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;

        public ProductController(IRepositoryManager repository, IWebHostEnvironment env) : base(repository)
        {
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return Ok(await _repository.Product.GetAll(false));
        }

        [HttpGet("{id}")]
        public async Task<Product> GetProduct(int id)
        {
            return await _repository.Product.GetById(id,true);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductDto product)
        {
            try
            {
                _repository.BeginTransaction();
                var entity = _repository.Product.Create(new Product(){Name = product.Name, DefaultQuantity = (int)product.DefaultQuantity});
                await _repository.SaveAsync();
                
                if(product.Picture != null && product.Picture.Length > 0)
                {
                    var path = _env.WebRootPath + "\\Images\\" + entity.Id.ToString()+product.PictureExntension;
                    
                    System.IO.File.WriteAllBytes(path, product.Picture);

                    _repository.Image.Create(new Image()
                    {
                        Id = entity.Id,
                        Path = entity.Id.ToString()+product.PictureExntension
                    });
                }
                
                await _repository.SaveAsync();
                _repository.Commit();

                return Ok();
            }
            catch (Exception ex)
            {
                _repository.Rollback();   
                return BadRequest(ex.Message);
            }    
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateProduct(int id, ProductEditDto product)
        {
            var entity = await _repository.Product.GetById(id, true);

            if(entity == null)
                return NotFound(ApiException.GetErrorMessage(ApiException.Errors.ProductNotFound));

            entity.Name = product.Name;
            entity.DefaultQuantity = (int)product.DefaultQuantity;

            if(product.Picture!= null && product.Picture.Length > 0)
            { 
                var oldFileName = entity.Image?.Path;
                var fileName = entity.Id.ToString()+product.PictureExntension;
                var path = _env.WebRootPath + "\\Images\\";
                    
                System.IO.File.WriteAllBytes(path + fileName, product.Picture);

                if(entity.Image == null)
                {
                    _repository.Image.Create(new Image()
                    {
                        Id = entity.Id,
                        Path = fileName
                    });
                }
                else
                    entity.Image.Path = fileName;

                if(fileName != oldFileName)
                    if(System.IO.File.Exists(path + oldFileName))
                        System.IO.File.Delete(path + oldFileName);
            }

            _repository.Save();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DelProduct(int id)
        {
            var product = await _repository.Product.GetById(id,true);

            if(product == null)
                return NotFound(ApiException.GetErrorMessage(ApiException.Errors.ProductNotFound));

            

            if(product.Image != null)
            {
                var path = _env.WebRootPath + "\\Images\\" + product.Image.Path; 

                if(System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
            }

            _repository.Product.Delete(product);
            _repository.Save();

            return Ok();
        }
    }
}