using JobSeaAPI.Database;
using JobSeaAPI.Exceptions;
using JobSeaAPI.Models;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
            if (filter is not null) query = query.Where(filter);
            
            return query.ToList();
        }

        public T GetEntity(Expression<Func<T, bool>>? filter)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return query.FirstOrDefault();
        }

        public async Task<T> CreateEntity(T newEntity)
        {
            try
            {
                _db.Set<T>().Add(newEntity);
                await _db.SaveChangesAsync();
                return newEntity;
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            
        }
        public async Task<bool> DeleteEntities(List<T> entities)
        {
            try
            {
                dbSet.RemoveRange(entities);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteEntity(T entity)
        {
            try
            {
                if(entity is not null)
                {
                    dbSet.Remove(entity);
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (DbUpdateException ex) 
            {
                return false;
            }
        }
    }
}