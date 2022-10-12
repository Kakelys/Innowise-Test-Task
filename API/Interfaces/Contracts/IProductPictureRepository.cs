using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces.Contracts
{
    public interface IProductPictureRepository : IRepositoryBase<Image>
    {
        Task<Image> GetByIdAsync(int id, bool asTracking);
    }
}