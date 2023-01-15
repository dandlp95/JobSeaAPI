using JobSeaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobSeaAPI.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Status> Status { get; set; }
    }
}
