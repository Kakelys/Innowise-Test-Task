using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Interfaces;
using API.Interfaces.Logics;
using Microsoft.AspNetCore.Hosting;

namespace API.Services.Logics
{
    public class ProductService : ServiceBase, IProductService
    {
        private readonly IWebHostEnvironment _env;
        public ProductService(IRepositoryManager repository, IWebHostEnvironment env) : base(repository)
        {
            _env = env;
        }

        public Task<IEnumerable<Product>> GetProducts(bool asTracking = false)
        {
            return _repository.Product.GetAll(asTracking);
        }

        public Task<Product> GetProductById(int id, bool asTracking)
        {
            return _repository.Product.GetById(id, asTracking);
        }

        public Product Add(ProductDto product)
        {
            try
            {
                _repository.BeginTransaction();
                var entity = _repository.Product.Create(new Product(){Name = product.Name, DefaultQuantity = (int)product.DefaultQuantity});
                _repository.Save();
                
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
                
                _repository.Save();
                _repository.Commit();

                return entity;
            }
            catch (Exception)
            {
                _repository.Rollback();   
                throw new ApiException(400, "Something went wrong while creating Product");
            }
        }

        public async Task<bool> Delete(int id)
        {
            var product = await _repository.Product.GetById(id,true);

            if(product == null)
                throw new ApiException(404, ApiException.GetErrorMessage(ApiException.Errors.ProductNotFound));

            if(product.Image != null)
            {
                var path = _env.WebRootPath + "\\Images\\" + product.Image.Path; 

                if(System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
            }

            _repository.Product.Delete(product);
            _repository.Save();

            return true;
        }       

        public async Task<bool> Update(int id, ProductEditDto product)
        {
            var entity = await _repository.Product.GetById(id, true);

            if(entity == null)
                throw new ApiException(404, ApiException.GetErrorMessage(ApiException.Errors.ProductNotFound));

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

            return true;
        }
    }
}