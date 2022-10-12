using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces.Contracts
{
    public interface IFridgeModelRepository : IRepositoryBase<FridgeModel>
    {
        Task<IEnumerable<FridgeModel>> GetAll(bool asTracking);
    }
}