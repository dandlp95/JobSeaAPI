using JobSeaAPI.Database;
using JobSeaAPI.Exceptions;
using JobSeaAPI.Models;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

/*
IQueryable provides deferred execution, which means that the query is not executed until 
an action is performed that requires the actual data, such as iterating over the results, 
calling a method like ToList(), or using an aggregation function like Count().
*/

namespace JobSeaAPI.Repository
{
    public abstract class Repository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly ILoggerCustom _logger;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db, ILoggerCustom logger) {
            _db = db;
            _logger = logger;
            dbSet = _db.Set<T>();
        }
        
        public List<T> GetAllEntities(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return query.ToList();
        }
        
        public T? GetEntity(Expression<Func<T, bool>>? filter)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return query.FirstOrDefault();
        }

        public async Task CreateEntity<E>(E newEntity) where E : class
        {
            _db.Set<E>().Add(newEntity);
            await _db.SaveChangesAsync();
        }
    }
}

//IQueryable<T> query = dbSet;
//if(filter != null)
//{
//    query = query.Where(filter);
//}

//return await query.ToListAsync();