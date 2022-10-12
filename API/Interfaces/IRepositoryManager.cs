using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces.Contracts;

namespace API.Interfaces
{
    public interface IRepositoryManager
    {
        IFridgeRepository Fridge {get;}
        IFridgeProductRepository FridgeProduct {get;}
        IFridgeModelRepository FridgeModel {get;}
        IProductRepository Product{get;}
        IProductPictureRepository Image {get;}
        IUserRepository User {get;}
        IRoleRepository Role {get;}
        void BeginTransaction();
        void Commit();
        void Rollback();
        int Save();
        Task<int> SaveAsync();
    }
}