using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces.Contracts;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class ProductPictureRepository : RepositoryBase<Image>, IProductPictureRepository
    {
        public ProductPictureRepository(RepositoryContext context) : base(context)
        {
        }

        public Task<Image> GetByIdAsync(int id, bool asTracking) =>
            FindByCondition(i => i.Id == id, asTracking).FirstOrDefaultAsync();
        
    }
}