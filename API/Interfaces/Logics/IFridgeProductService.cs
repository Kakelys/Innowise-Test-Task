using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces.Logics
{
    public interface IFridgeProductService
    {
        Task<IEnumerable<FridgeProduct>> GetProducts(int fridgeId, bool asTracking = false);
        Task<FridgeProduct> GetProduct(int fridgeId, int productId, bool asTracking);
        Task<bool> Add(int fridgeId, FridgeProductDto product);
        Task<bool> Take(int productId, FridgeProductDto product);
        Task<bool> Delete(int productId);
        Task<bool> Update(int productId, FridgeProductDto product);
    }
}