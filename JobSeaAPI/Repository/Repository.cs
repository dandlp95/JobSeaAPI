using JobSeaAPI.Database;
using JobSeaAPI.Exceptions;
using JobSeaAPI.Models;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
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

        protected List<T> GetAllEntities(Expression<Func<T, bool>>? filter = null, int? skip = null, int? rows = null)
        {
            IQueryable<T> query = dbSet;
            if (filter is not null) query = query.Where(filter);
            
            if (skip is not null && rows is not null)
            {
                query.Skip((int)skip).Take((int)rows);
            }

            return query.ToList();
        }

        protected T? GetEntity(Expression<Func<T, bool>>? filter)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return query.FirstOrDefault();
        }

        protected async Task<T> CreateEntity(T newEntity)
        {
            _db.Set<T>().Add(newEntity);
            await _db.SaveChangesAsync();
            return newEntity;

        }
        protected async Task DeleteEntities(List<T> entities)
        {
            dbSet.RemoveRange(entities);
            await _db.SaveChangesAsync();
        }
        protected async Task<bool> DeleteEntity(T entity)
        {
            if(entity is not null)
            {
                dbSet.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        protected async Task<T> UpdateEntity<K>(T entity, K entityDTO, bool updateAllFields = false) where K : class , new() 
        {   
            if(entity is null ||  entityDTO is null) throw new Exception("Entities cannot be null.");

            if (updateAllFields is true)
            {
                dbSet.Entry(entity).CurrentValues.SetValues(entityDTO);
            }
            else
            {
                Type entityDTOType = typeof(K);
                Type entityType = typeof(T);

                PropertyInfo[] dtoProperties = entityDTOType.GetProperties();

                foreach (PropertyInfo dtoProperty in dtoProperties)
                {
                    object? dtoValue = dtoProperty.GetValue(entityDTO);

                    if (dtoValue is not null)
                    {
                        PropertyInfo? entityProperty = entityType.GetProperty(dtoProperty.Name);

                        entityProperty?.SetValue(entity, dtoValue);
                    }
                }
            }
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}