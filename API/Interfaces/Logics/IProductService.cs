using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces.Logics
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts(bool asTracking = false);
        Task<Product> GetProductById(int id, bool asTracking);
        Product Add(ProductDto product);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, ProductEditDto product);
    }
}