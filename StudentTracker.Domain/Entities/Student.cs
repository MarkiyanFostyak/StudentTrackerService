using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTracker.Domain.Entities
{
    public class Student
    {
        public Student()
        {
            Courses = new List<Course>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public int Age { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }

    public enum Sex
    {
        Female,
        Male
    }
}
