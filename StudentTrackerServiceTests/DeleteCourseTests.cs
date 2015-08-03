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
    public class DeleteCourseTests
    {
        private StudentTrackerService.StudentTrackerService stService;
        private string notExistingId = "-1";
        private Course math = new Course { CourseName = "MathematicsTest", MaxNumberOfStudents = 15 };
        private Course calculus = new Course { CourseName = "CalculusTest", MaxNumberOfStudents = 10 };
        private Course discrete = new Course { CourseName = "DiscreteTest", MaxNumberOfStudents = 25 };

        private int initCount;

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

                initCount = ctx.Courses.Count();
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
        public void TestDeleteSuccess()
        {
            stService.DeleteCourse(math.Id.ToString());
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.IsNull(ctx.Courses.Find(math.Id));
            }
        }

        [Test]
        public void TestDeleteNoId()
        {
            stService.DeleteCourse(notExistingId);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(initCount, ctx.Courses.Count());
            }
        }
    }
}
