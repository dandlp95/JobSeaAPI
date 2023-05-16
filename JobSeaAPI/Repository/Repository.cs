using JobSeaAPI.Database;
using JobSeaAPI.Exceptions;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace JobSeaAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        private readonly ILoggerCustom _logger;
        public Repository(ApplicationDbContext db, ILoggerCustom logger) {
            _db = db;
            dbSet = _db.Set<T>();
            _logger = logger;
        }
        public async Task CreateAsync(T entity)
        {
            try
            {
                if(entity == null)
                {
                    throw new JobSeaException(HttpStatusCode.BadRequest, "User can't be null.");
                }
                await dbSet.AddAsync(entity);
                await SaveAsync();
            } catch (Exception ex)
            {
                _logger.Log(ex.ToString(), "Create Async Repo error: ");
                //throw new JobSeaException(HttpStatusCode.Conflict, ex.Message);
                throw new JobSeaException(HttpStatusCode.Conflict, "We entered the exception!");
            }
        }

        public async Task DeleteAsync(T entity)
        {
            await dbSet.ExecuteDeleteAsync(); // Does not track entity and executes deletion without having to call SaveChangesAsync
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if(filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
