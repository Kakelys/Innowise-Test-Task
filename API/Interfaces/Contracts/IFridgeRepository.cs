using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces.Contracts
{
    public interface IFridgeRepository : IRepositoryBase<Fridge>
    {
        Task<IEnumerable<Fridge>> GetAllFridgesAsync(bool asTracking);
        Task<Fridge> GetFridgeDetail(int? id, bool asTracking);
        Task<Fridge> GetByIdAsync(int? id, bool asTracking);
        
    }
}