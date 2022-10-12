using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces.Contracts
{
    public interface IFridgeProductRepository : IRepositoryBase<FridgeProduct>
    {
        Task<IEnumerable<FridgeProduct>> GetAllByFridgeId(int? id, bool asTracking);
        Task<FridgeProduct> GetByProductId(int? id, int fridgeId,bool asTracking);
        Task<FridgeProduct> GetById(int? id,bool asTracking);
        Task<FridgeProduct> GetByIdWithProduct(int? id, bool asTracking);
        Task<IEnumerable<FridgeProduct>> FindEmptyAsync();
    }
}