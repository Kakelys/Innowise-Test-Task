using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces.Contracts;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class FridgeModelRepository : RepositoryBase<FridgeModel>, IFridgeModelRepository
    {
        public FridgeModelRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FridgeModel>> GetAll(bool asTracking) =>
            await FindAll(asTracking).ToListAsync();
        
    }
}