using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<bool> IsUserExist(string name);
        Task<User> GetByNameAsync(string name, bool asTracking);
        
    }
}