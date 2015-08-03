using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTracker.Domain.Entities;

namespace StudentTracker.Domain.Configurations
{
    public class CourseConfigurations: EntityTypeConfiguration<Course>
    {
        public CourseConfigurations()
        {
            this.ToTable("Courses");
            this.HasKey(x => x.Id);
        }
    }
}
