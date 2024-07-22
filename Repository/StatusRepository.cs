using AutoMapper;
using JobSeaAPI.Database;
using JobSeaAPI.Models;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace JobSeaAPI.Repository
{
    public class StatusRepository:Repository<Status>, IStatusRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly DbSet<Status> dbSet;
        public StatusRepository(ApplicationDbContext db, IMapper mapper, ILoggerCustom logger):base(db, logger)
        {
            _db = db;
            _mapper = mapper;
            dbSet = db.Set<Status>();
        }

        public List<Status> GetStatuses()
        {
            List<Status> statuses = GetAllEntities();
            return statuses;
        }
    }
}
