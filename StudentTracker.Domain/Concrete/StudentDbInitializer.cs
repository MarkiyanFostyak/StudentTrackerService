using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTracker.Domain.Concrete
{
    public class StudentDbInitializer: DropCreateDatabaseIfModelChanges<StudentTrackerDbContext>
    {
        protected override void Seed(StudentTrackerDbContext context)
        {
            base.Seed(context);
        }
    }
}
