using System;
using NUnit;
using NUnit.Framework;
using Moq;
using StudentTracker.Domain;
using StudentTracker.Domain.Concrete;
using StudentTracker.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using StudentTrackerService;
using System.Data.Entity; 

namespace StudentTrackerServiceTests
{
    [TestFixture]
    public class GetCoursesTest
    {
        private StudentTrackerService.StudentTrackerService stService;
        private Course math = new Course { CourseName = "MathematicsTest", MaxNumberOfStudents = 15 };
        private Course calculus = new Course { CourseName = "CalculusTest", MaxNumberOfStudents = 10 };
        private Course discrete = new Course { CourseName = "DiscreteTest", MaxNumberOfStudents = 25 };
        private List<CourseInfo> expectedCourses;

        [SetUp]
        public void Initialize()
        {
            this.stService = new StudentTrackerService.StudentTrackerService();
            using (var ctx = new StudentTrackerDbContext())
            {
                ctx.Courses.Add(math);
                ctx.Courses.Add(calculus);
                ctx.Courses.Add(discrete);
                ctx.SaveChanges(); 
                expectedCourses = new List<CourseInfo>();
                foreach (var course in ctx.Courses)
                {
                    expectedCourses.Add(new CourseInfo { CourseName = course.CourseName, Id = course.Id, CurrentlyAssigned = course.Students.Count(), MaxAssigned = course.MaxNumberOfStudents });
                }
            }

        }

        [TearDown]
        public void CleanUp()
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                foreach (var cs in ctx.Courses.Where(x => x.CourseName == math.CourseName || x.CourseName == discrete.CourseName || x.CourseName == calculus.CourseName))
                {
                    ctx.Courses.Remove(cs);
                }
                ctx.SaveChanges();
            }
        }

        [Test]
        public void GetAll()
        {
            var result = stService.GetCourses();
            Assert.IsTrue(expectedCourses.SequenceEqual(result));
        }
    }
}
