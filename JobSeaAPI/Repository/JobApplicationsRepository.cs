using AutoMapper;
using JobSeaAPI.Database;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using System.Linq.Expressions;
using JobSeaAPI.Exceptions;

namespace JobSeaAPI.Repository
{
    public class JobApplicationsRepository : Repository<Application>, IJobApplicationsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUpdateRepository _updateRepo;
        public JobApplicationsRepository(ApplicationDbContext db, ILoggerCustom logger, IMapper mapper, IUpdateRepository updateRepo) : base(db, logger)
        {
            _db = db;
            _mapper = mapper;
            _updateRepo = updateRepo;   
        }


        public async Task<ApplicationDTO> CreateApplication(CreateApplicationDTO applicationDTORequest)
        {
            Application application = _mapper.Map<Application>(applicationDTORequest);
            application.Created = DateTime.Now;
            application.LastUpdated = DateTime.Now;

            await CreateEntity(application);
            await _updateRepo.CreateUpdate(applicationDTORequest.firstUpdate, application);
            ApplicationDTO applicationDTO = _mapper.Map<ApplicationDTO>(application);
            return applicationDTO;
        }
        public List<Application> GetAllApplications(int userId)
        {
            Expression<Func<Application, bool>> filter = entity => entity.UserId == userId;
            List<Application> results = GetAllEntities(filter);
            return results;
        }


        public async Task DeleteApplication(int applicationId) 
        {
                List<Update> updatesToDelete = await _db.Set<Update>().Where(u => u.ApplicationId == applicationId).ToListAsync();
                await _updateRepo.DeleteUpdates(updatesToDelete);

                Application application = await dbSet.Where(a => a.ApplicationId == applicationId).FirstOrDefaultAsync();
                await DeleteEntity(application);
        }


        public Task<Application> GetApplication(string sqlQuery)
        {
            throw new NotImplementedException();
        }
        public Application GetApplication(int applicationId)
        {
            Expression<Func<Application, bool>> filter = entity => entity.ApplicationId == applicationId;
            Application application = GetEntity(filter);
            return application;
        }

        public async Task<Application> UpdateApplication(UpdateApplicationDTO applicationDTO)
        {
            Application? application = GetEntity(e => e.ApplicationId == applicationDTO.ApplicationId) ?? throw new JobSeaException(System.Net.HttpStatusCode.BadRequest, "ApplicationId doesn't match any entity in the database.");
            await UpdateEntity(application, applicationDTO);
            return application;
        }

    }
}
