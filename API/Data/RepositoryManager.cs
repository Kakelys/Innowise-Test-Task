using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Repositories;
using API.Interfaces;
using API.Interfaces.Contracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Data
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private IFridgeRepository _fridgeRepository;
        private IFridgeModelRepository _fridgeModelRepository;
        private IFridgeProductRepository _fridgeProductRepository;
        private IProductRepository _productRepository;
        private IProductPictureRepository _productPictureRepository;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IDbContextTransaction _transaction;

        public RepositoryManager(RepositoryContext context,
            IFridgeRepository fridgeRepository,
            IFridgeModelRepository fridgeModelRepository,
            IFridgeProductRepository fridgeProductRepository,
            IProductRepository productRepository,
            IProductPictureRepository productPictureRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _context = context;

            _fridgeRepository = fridgeRepository;
            _fridgeModelRepository = fridgeModelRepository;
            _fridgeProductRepository = fridgeProductRepository;
            _productRepository = productRepository;
            _productPictureRepository = productPictureRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public IFridgeRepository Fridge => _fridgeRepository;

        public IFridgeProductRepository FridgeProduct => _fridgeProductRepository;

        public IFridgeModelRepository FridgeModel => _fridgeModelRepository;

        public IProductRepository Product => _productRepository;

        public IProductPictureRepository Image => _productPictureRepository;

        public IUserRepository User => _userRepository;

        public IRoleRepository Role => _roleRepository;

        public void BeginTransaction() => 
            _transaction = _context.Database.BeginTransaction();

        public void Commit() => 
            _transaction.Commit();
            
        public void Rollback() =>
            _transaction.Rollback();
        
        public Task<int> SaveAsync() => _context.SaveChangesAsync();

        public int Save() => _context.SaveChanges();
    }
}