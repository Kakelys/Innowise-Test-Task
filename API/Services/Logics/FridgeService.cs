using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Interfaces;
using API.Interfaces.Logics;
using Newtonsoft.Json;

namespace API.Services.Logics
{
    public class FridgeService : ServiceBase, IFridgeService
    {
        public FridgeService(IRepositoryManager repository) : base(repository)
        {
        }

        public Task<IEnumerable<Fridge>> GetFridges(bool asTracking)
        {
            return _repository.Fridge.GetAllFridgesAsync(asTracking);
        }

        public async Task<Fridge> GetFridgeById(int id, bool asTracking)
        {
            var fridge = await _repository.Fridge.GetByIdAsync(id, asTracking);

            if(fridge == null)
                throw new ApiException(404, ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            return fridge;
        }

        public Fridge Add(CreateFridgeDto fridge)
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

                return fr;
            }
            catch
            {
                _repository.Rollback();
                throw new ApiException(400, "Something went wrong when building a new refrigerator");
            }
        }

        public async Task<bool> Delete(int id)
        {
            var fridge = await _repository.Fridge.GetByIdAsync(id, true);

            if(fridge == null)
                new ApiException(404, ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            _repository.Fridge.Delete(fridge);
                
            return _repository.Save() > 0;
        }

        public async Task<Fridge> GetFridgeDetail(int id)
        {
            var fridge = await _repository.Fridge.GetFridgeDetail(id, false);
            
            if(fridge == null)
               throw new ApiException(404, ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            return fridge;
        }

        public async Task<bool> IsFridgeExist(int id)=>
            await _repository.Fridge.GetByIdAsync(id, false) != null;

        public async Task<Fridge> Update(int id, EditFridgeDto fridge)
        {
            var fr = await _repository.Fridge.GetByIdAsync(id, true);

            if(fr == null)
                throw new ApiException(404, ApiException.GetErrorMessage(ApiException.Errors.InvalidFridgeId));

            fr.Name = fridge.Name;
            fr.OwnerName = fridge.OwnerName;
            fr.Model = fridge.Model;

            _repository.Save();

            return fr;
        }

        public async Task<bool> UpdateFridgesProducts()
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

            return true;
        }
    }
}