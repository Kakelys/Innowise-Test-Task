using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces.Contracts;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class FridgeProductRepository : RepositoryBase<FridgeProduct>, IFridgeProductRepository
    {
        public FridgeProductRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FridgeProduct>> FindEmptyAsync() =>
            await context.FridgeProducts.FromSqlRaw("EXEC uspFindEmpty;").ToListAsync();

        public async Task<IEnumerable<FridgeProduct>> GetAllByFridgeId(int? id, bool asTracking) =>
            await FindByCondition(fp => fp.Fridge == id, asTracking).ToListAsync();

        public Task<FridgeProduct> GetByProductId(int? id, int fridgeId, bool asTracking) =>
            FindByCondition(fp => fp.Product == id && fp.Fridge == fridgeId, asTracking).FirstOrDefaultAsync();

        public Task<FridgeProduct> GetById(int? id, bool asTracking) =>
            FindByCondition(fp => fp.Id == id, asTracking).FirstOrDefaultAsync();

        public Task<FridgeProduct> GetByIdWithProduct(int? id, bool asTracking) =>
            FindByCondition(fp => fp.Id == id, asTracking)
                .Include(fp => fp.ProductNavigation)
                .FirstOrDefaultAsync();
    }
}