using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool asTracking);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool asTracking);
        T Create(T entity);
        void CreateRange(IEnumerable<T> entities);
        void Update(T entity);
        bool Delete(T entity);
    }
}