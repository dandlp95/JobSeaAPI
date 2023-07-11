﻿using AutoMapper;
using JobSeaAPI.Database;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using System.Linq.Expressions;

namespace JobSeaAPI.Repository
{
    public class JobApplicationsRepository : Repository<Application>, IJobApplicationsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public JobApplicationsRepository(ApplicationDbContext db, ILoggerCustom logger, IMapper mapper) : base(db, logger)
        {
            _db = db;
            _mapper = mapper;
        }


        public ApplicationDTO CreateApplication(CreateApplicationDTO applicationDTORequest)
        {
            Application application = _mapper.Map<Application>(applicationDTORequest);
            application.Created = DateTime.Now;
            application.LastUpdated = DateTime.Now;

            Update update = _mapper.Map<Update>(applicationDTORequest.firstUpdate);
            update.Created = DateTime.Now;
            update.Application = application;

            _db.Set<Application>().Add(application);
            _db.Set<Update>().Add(update);
            _db.SaveChanges();

            ApplicationDTO applicationDTO = _mapper.Map<ApplicationDTO>(application);
            return applicationDTO;
        }
        public List<Update> GetAllUpdates(int userId, int applicationId)
        {
            Expression<Func<Update, bool>> queryExpression = entity =>
                entity.ApplicationId == applicationId &&
                entity.Application.UserId == userId;

            IQueryable<Update> query = _db.Set<Update>();
            query = query.Where(queryExpression).Include(u=>u.Status);

            return query.ToList();
        }
        public List<Application> GetAllApplications(int userId)
        {
            Expression<Func<Application, bool>> filter = entity => entity.UserId == userId;
            List<Application> results = GetAllEntities(filter);
            return results;
        }


        public Task DeleteApplication(Application application) 
        {
            throw new NotImplementedException();
        }


        public Task<Application> GetApplication(string sqlQuery)
        {
            throw new NotImplementedException();
        }

        public Task UpdateApplication(Application application)
        {
            throw new NotImplementedException(); 
        }

    }
}
