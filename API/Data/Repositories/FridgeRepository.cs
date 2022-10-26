using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces.Contracts;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class FridgeRepository : RepositoryBase<Fridge>, IFridgeRepository
    {
        public FridgeRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Fridge>> GetAllFridgesAsync(bool asTracking) =>
            await FindAll(asTracking)
                .Include(f=>f.ModelNavigation)
                .ToListAsync();

        public Task<Fridge> GetByIdAsync(int? id, bool asTracking) =>
             FindByCondition(fr => fr.Id == id, asTracking)
                .Include(fr => fr.ModelNavigation)
                .FirstOrDefaultAsync();

        public Task<Fridge> GetFridgeDetail(int? id, bool asTracking)=>
            FindByCondition(fr => fr.Id == id, asTracking)
                .Include(fr => fr.ModelNavigation)
                .Include(fr => fr.FridgeProducts)
                .ThenInclude(fp => fp.ProductNavigation)
                .FirstOrDefaultAsync();

    }
}