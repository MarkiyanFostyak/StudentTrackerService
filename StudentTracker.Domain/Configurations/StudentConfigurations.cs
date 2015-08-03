using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTracker.Domain.Entities;

namespace StudentTracker.Domain.Configurations
{
    public class StudentConfigurations: EntityTypeConfiguration<Student>
    {
        public StudentConfigurations()
        {
            this.ToTable("Students");
            this.HasKey(x => x.Id);
            this.HasMany<Course>(s => s.Courses)
                .WithMany(c => c.Students)
                .Map(cs =>
                {
                    cs.MapLeftKey("StudentRefId");
                    cs.MapRightKey("CourseRefId");
                    cs.ToTable("StudentsCourses");
                });
        }
    }
}
