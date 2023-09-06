using JobSeaAPI.Database;
using JobSeaAPI.Models;
using JobSeaAPI.Repository.IRepository;
using JobSeaAPI.Services;

namespace JobSeaAPI.Repository
{
    public class ModalityRepository : Repository<Modality>, IModalityRepository
    {
        private readonly ApplicationDbContext _db;
        public ModalityRepository(ApplicationDbContext db, ILoggerCustom _logger) : base(db, _logger)
        {
            _db = db;        
        }
        public List<Modality> GetModalities()
        {
            List<Modality> modalities = GetAllEntities();
            return modalities;
        }
    }
}
