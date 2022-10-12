using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces.Contracts;

namespace API.Data.Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(RepositoryContext context) : base(context)
        {
        }
    }
}