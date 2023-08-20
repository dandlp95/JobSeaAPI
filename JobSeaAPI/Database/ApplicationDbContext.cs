using JobSeaAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

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
        public DbSet<Update> Updates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>();
            modelBuilder.Entity<Application>();
            modelBuilder.Entity<Update>();
            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = 1,StatusName = "Hired" },
                new Status { StatusId = 2, StatusName = "Rejected" },
                new Status { StatusId = 3, StatusName = "Interview Scheduled" },
                new Status { StatusId = 4, StatusName = "Applied"},
                new Status { StatusId = 5, StatusName = "Waiting"}
                );
        }

    }
}

/*
To perform a database migration using .NET Core, you can use the following steps:

Open a terminal/command prompt and navigate to the project directory where your .NET Core project is located.

Ensure that the Entity Framework Core tool is installed by running the following command:

dotnet tool install --global dotnet-ef

Ensure that your project has a reference to the Entity Framework Core package. You can check this by looking at the project's .csproj file.

Next, you will need to add a migration to your project. To do this, run the following command, replacing YourMigrationName with a name of your choice:

dotnet ef migrations add YourMigrationName

This command will generate a new migration file in the project's Migrations directory.

Finally, to apply the migration and update your database, run the following command:

dotnet ef database update

If you encounter any errors during the migration process, you may need to troubleshoot the issue by reviewing error messages or updating your project configuration. 
Additionally, you may want to consider backing up your database before performing any migrations to avoid data loss.
*/