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
        DbSet<Update> _dbSet;
        public UpdateRepository(ApplicationDbContext db, IMapper mapper, ILoggerCustom logger) : base(db, logger)
        {
            _db = db;
            _mapper = mapper;
            _dbSet = _db.Set<Update>();
        }

        public async Task<Update> CreateUpdate(UpdateCreateDTO updateDTO, Application application)
        {
            Update update = _mapper.Map<Update>(updateDTO);
            update.Application = application;
            update.Created = DateTime.Now;
            await CreateEntity(update);
            return update;
        }
        public async Task<Update> CreateUpdate(UpdateCreateDTO updateDTO)
        {
            Update update = _mapper.Map<Update>(updateDTO);
            update.Created = DateTime.Now;
            await CreateEntity(update);
            return update;
        }

        public async Task<bool> DeleteUpdate(Update update)
        {
            bool operationResult = await DeleteEntity(update);

            return operationResult; 
        }
        public async Task<bool> DeleteUpdate(int updateId)
        {
            Expression<Func<Update, bool>> expression = entity => entity.UpdateId == updateId;
            Update update = GetEntity(expression);
            bool operationResult = await DeleteEntity(update);

            return operationResult;
        }

        public async Task<bool> DeleteUpdates(List<Update> updates)
        {
            bool operationResult = await DeleteEntities(updates);

            return operationResult;
        }


        public List<Update> GetUpdates(int userId, int applicationId)
        {
            Expression<Func<Update, bool>> queryExpression = entity =>
                entity.ApplicationId == applicationId &&
                entity.Application.UserId == userId;

            IQueryable<Update> query = _dbSet;
            query = query.Where(queryExpression).Include(u => u.Status);

            return query.ToList() ?? new List<Update>();
        }
        public Update GetUpdate(int updateId)
        {
            Expression<Func<Update,bool>> expression = entity => entity.UpdateId == updateId;
            Update update = GetEntity(expression);
            return update;
        }
        public async Task<Update> UpdateUpdate(UpdateUpdateDTO updateDTO)
        {

            throw new NotImplementedException();
        }
    }
}
