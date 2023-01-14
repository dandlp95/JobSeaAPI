using JobSeaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobSeaAPI.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> users { get; set; }
        public DbSet<Application> applications { get; set; }
        public DbSet<Status> status { get; set; }
    }
}
