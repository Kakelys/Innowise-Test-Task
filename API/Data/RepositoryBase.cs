using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly RepositoryContext context;
        public RepositoryBase(RepositoryContext context)
        {
            this.context = context;
        }

        public IQueryable<T> FindAll(bool asTracking)=>
            asTracking ?
                context.Set<T>() :
                context.Set<T>()
                    .AsNoTracking();
        

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool asTracking) =>
            asTracking ?
                context.Set<T>()
                    .Where(expression) :
                context.Set<T>()
                    .Where(expression)
                    .AsNoTracking();

        public T Create(T entity) =>
            context.Set<T>().Add(entity).Entity;

        public bool Delete(T entity)
        {
            try
            {
                context.Set<T>().Remove(entity);
            }
            catch
            {
                return false;
            }
            return true;
        }
            

        public void Update(T entity) => 
            context.Set<T>().Update(entity);

        public void CreateRange(IEnumerable<T> entities) =>
            context.Set<T>().AddRange(entities);
    }
}