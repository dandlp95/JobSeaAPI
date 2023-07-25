using AutoMapper;
using JobSeaAPI.Database;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace JobSeaAPI.Repository
{
    public class UpdateRepository:Repository<Update>,IUpdateRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        DbSet<Update> dbSet;
        public UpdateRepository(ApplicationDbContext db, IMapper mapper, ILoggerCustom logger):base(db, logger)
        {
            _db = db;
            _mapper = mapper;
            DbSet<Update> dbSet = _db.Set<Update>();
        }

        public async Task<Update> CreateUpdate(UpdateCreateDTO updateDTO, Application application)
        {
            Update update = _mapper.Map<Update>(updateDTO);
            update.Application = application;
            update.Created = DateTime.Now;
            await CreateEntity(update);
            return update;
        }
        public async Task<Update> CreateUpdate(UpdateCreateDTO updateDTO, int applicationId)
        {
            Update update = _mapper.Map<Update>(updateDTO);
            update.ApplicationId = applicationId;
            update.Created = DateTime.Now;
            await CreateEntity(update);
            return update;
        }


        public async Task<bool> DeleteUpdate(List<Update> updates)
        {
            bool operationResult = await DeleteEntities(updates);
            return operationResult; 
        }

        public List<Update> GetUpdates(int userId, int applicationId)
        {
            Expression<Func<Update, bool>> queryExpression = entity =>
                entity.ApplicationId == applicationId &&
                entity.Application.UserId == userId;

            IQueryable<Update> query = dbSet;
            query = query.Where(queryExpression).Include(u => u.Status);

            return query.ToList();
        }
    }
}
