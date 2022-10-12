using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces.Contracts;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<User> GetByNameAsync(string name, bool asTracking) =>
            await FindByCondition(u=>u.Name == name, asTracking).FirstOrDefaultAsync();

        public async Task<bool> IsUserExist(string name) =>
            await context.Users.FirstOrDefaultAsync(u => u.Name == name) == null ? false : true;
    }
}