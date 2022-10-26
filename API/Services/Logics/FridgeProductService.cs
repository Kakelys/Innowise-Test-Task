using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Interfaces;
using API.Interfaces.Logics;

namespace API.Services.Logics
{
    public class FridgeProductService : ServiceBase, IFridgeProductService
    {
        private IFridgeService _fridgeService;

        public FridgeProductService(IRepositoryManager repository, IFridgeService fridgeService) : base(repository)
        {
            _fridgeService = fridgeService;
        }

        public async Task<IEnumerable<FridgeProduct>> GetProducts(int fridgeId, bool asTracking)
        {
            return await _repository.FridgeProduct.GetAllByFridgeId(fridgeId, asTracking);
        }

        public async Task<FridgeProduct> GetProduct(int fridgeId ,int productId, bool asTracking)
        {
            if(!(await _fridgeService.IsFridgeExist(fridgeId)))
                throw new ApiException(404, ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            var product = await _repository.FridgeProduct.GetByIdWithProduct(productId, asTracking);
            
            if(product == null)
                throw new ApiException(404, ApiException.GetErrorMessage(ApiException.Errors.ProductNotFound));

            return product;
        }

        public async Task<bool> Add(int fridgeId, FridgeProductDto product)
        {
            if(!(await _fridgeService.IsFridgeExist(fridgeId)))
               throw new ApiException(400, ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            var fp = await _repository.FridgeProduct.GetByProductId(product.ProductId, fridgeId, true);

            if(fp != null)
            {
                fp.Quantity += product.Quantity;
            }
            else
            {
                fp = _repository.FridgeProduct.Create(
                    new FridgeProduct()
                    {
                        Product = product.ProductId,
                        Quantity = product.Quantity,
                        Fridge = fridgeId
                    });
            }

            return _repository.Save() > 0;
        }

        public async Task<bool> Delete(int productId)
        {      
            var fp = await _repository.FridgeProduct.GetById(productId, false);

            if(fp == null)
                throw new ApiException(404, "There is no product, that matches the request");

            _repository.FridgeProduct.Delete(fp);

            return _repository.Save() > 0;
        }

        public async Task<bool> Take(int productId, FridgeProductDto product)
        {
            var fp = await _repository.FridgeProduct.GetById(productId, true);

            if(fp == null)
                throw new ApiException(404, ApiException.GetErrorMessage(ApiException.Errors.ProductNotFound));

            if(fp.Quantity - product.Quantity < 0)
                throw new ApiException(400, "Fridge doesn't have enough amount of products");

            fp.Quantity -= product.Quantity;
            if(fp.Quantity == 0)
                _repository.FridgeProduct.Delete(fp);

            return _repository.Save() > 0;
        }

        public async Task<bool> Update(int productId, FridgeProductDto product)
        {
            var entity = await _repository.FridgeProduct.GetById(productId, true);

            if(entity == null)
                throw new ApiException(404, ApiException.GetErrorMessage(ApiException.Errors.ProductNotFound));

            entity.Quantity = product.Quantity;

            return _repository.Save() > 0;
        }
    }
}