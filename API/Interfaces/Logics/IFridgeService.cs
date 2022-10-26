using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces.Logics
{
    public interface IFridgeService
    {
        Task<IEnumerable<Fridge>> GetFridges(bool asTracking = false);
        Task<Fridge> GetFridgeById(int id, bool asTracking);
        Task<Fridge> GetFridgeDetail(int id);
        Fridge Add(CreateFridgeDto fridge);
        Task<Fridge> Update(int id, EditFridgeDto fridge);
        Task<bool> Delete(int id);
        Task<bool> UpdateFridgesProducts();
        Task<bool> IsFridgeExist(int id);
    }
}