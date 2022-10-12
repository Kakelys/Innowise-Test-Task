using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces.Contracts;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAll(bool asTracking) => 
            await FindAll(asTracking)
                .Include(p=>p.Image)
                .ToListAsync();

        public Task<Product> GetById(int id, bool asTracking) =>
            FindByCondition(p => p.Id == id, asTracking)
            .Include(p => p.Image)
            .FirstOrDefaultAsync();
    }
}