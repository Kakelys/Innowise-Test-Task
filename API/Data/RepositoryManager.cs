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
        private IProductPictureRepository _imageRepository;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IDbContextTransaction _transaction;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
        }

        public IFridgeRepository Fridge
        {
            get
            {
                if(_fridgeRepository == null)
                    _fridgeRepository = new FridgeRepository(_context);
                
                return _fridgeRepository;
            }
        }

        public IFridgeProductRepository FridgeProduct
        {
             get
            {
                if(_fridgeProductRepository == null)
                    _fridgeProductRepository = new FridgeProductRepository(_context);
                
                return _fridgeProductRepository;
            }
        }

        public IFridgeModelRepository FridgeModel
        {
            get
            {
                if(_fridgeModelRepository == null)
                    _fridgeModelRepository = new FridgeModelRepository(_context);
                
                return _fridgeModelRepository;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if(_productRepository == null)
                    _productRepository = new ProductRepository(_context);
                
                return _productRepository;
            }
        }

        public IProductPictureRepository Image
        {
            get
            {
                if(_imageRepository == null)
                    _imageRepository = new ProductPictureRepository(_context);

                return _imageRepository;
            }
        }

        public IUserRepository User
        {
            get
            {
                if(_userRepository == null)
                    _userRepository = new UserRepository(_context);

                return _userRepository;
            }
        }

        public IRoleRepository Role
        {
            get
            {
                if(_roleRepository == null)
                    _roleRepository = new RoleRepository(_context);

                return _roleRepository;
            }
        }

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