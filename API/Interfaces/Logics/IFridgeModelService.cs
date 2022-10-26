using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces.Logics
{
    public interface IFridgeModelService
    {
        Task<IEnumerable<FridgeModel>> GetModels(bool asTracking = false);
    }
}