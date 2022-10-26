using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using API.Interfaces.Logics;

namespace API.Services.Logics
{
    public class FridgeModelService : ServiceBase, IFridgeModelService
    {
        public FridgeModelService(IRepositoryManager repository) : base(repository)
        {
        }

        public Task<IEnumerable<FridgeModel>> GetModels(bool asTracking)
        {
            return _repository.FridgeModel.GetAll(asTracking);
        }
    }
}