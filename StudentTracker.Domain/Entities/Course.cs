using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTracker.Domain.Entities
{
    public class Course
    {
        public Course()
        {
            Students = new List<Student>();
        }
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int MaxNumberOfStudents { get; set; }
        public virtual ICollection<Student> Students { get; set; }


    }
}
