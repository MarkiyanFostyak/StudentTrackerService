using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentTracker.Domain.Configurations;
using StudentTracker.Domain.Entities;
using StudentTracker.Domain.IdentityEntities;

namespace StudentTracker.Domain.Concrete
{
    public class StudentTrackerDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public StudentTrackerDbContext()
            : base("DefaultConnection")
        {           
        }

        public StudentTrackerDbContext(string connectionString)
            :base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new StudentConfigurations());
            modelBuilder.Configurations.Add(new CourseConfigurations());
            modelBuilder.Configurations.Add(new UiColourConfiguration());
        }
    }
}
