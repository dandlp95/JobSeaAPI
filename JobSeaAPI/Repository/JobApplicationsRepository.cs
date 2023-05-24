using AutoMapper;
using JobSeaAPI.Database;
using JobSeaAPI.Models;
using JobSeaAPI.Models.DTO;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;

namespace JobSeaAPI.Repository
{
    public class JobApplicationsRepository : Repository<Application>, IJobApplicationsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public JobApplicationsRepository(ApplicationDbContext db, ILoggerCustom logger, IMapper mapper) : base(db, logger)
        {
            _db = db;
            dbSet = db.Set<Application>();
            _mapper = mapper;
        }

        public async Task CreateApplication(CreateApplicationDTO applicationDTO, Update updateDTO)
        {   
            // Stopping point
            Application application = _mapper.Map<Application>(applicationDTO);
            Update update = _mapper.Map<Update>(updateDTO);
            await CreateAsync(application);
            update.
        }

        public Task DeleteApplication(Application application)
        {
            throw new NotImplementedException();
        }

        public Task<List<Application>> GetAllApplications()
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
