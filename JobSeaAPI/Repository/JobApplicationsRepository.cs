using AutoMapper;
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

        public async Task CreateApplication(CreateApplicationDTO applicationDTO, Update updateDTO)
        {   
            throw new NotImplementedException();
        }



        public void CreateApplication(Application application, Update update)
        {
            update.Application = application;

            _db.Set<Application>().Add(application);
            _db.Set<Update>().Add(update);
        }

        public Task DeleteApplication(Application application) 
        {
            throw new NotImplementedException();
        }

        public List<Application> GetAllApplications(int userId)
        {
            Expression<Func<Application, bool>> filter = entity => entity.UserId == userId;
            List<Application> results = GetAllEntities(filter);
            return results;
        }

        public Task<Application> GetApplication(string sqlQuery)
        {
            throw new NotImplementedException();
        }

        public Task UpdateApplication(Application application)
        {
            throw new NotImplementedException();
        }
        public List<Update> GetAllUpdates(int userId, int applicationId)
        {
            Expression<Func<Update, bool>> queryExpression = entity =>
                entity.ApplicationId == applicationId &&
                entity.Application.UserId == userId;

            List<Update> updates = GetAllEntities(queryExpression);
            return updates;
        }
    }
}
