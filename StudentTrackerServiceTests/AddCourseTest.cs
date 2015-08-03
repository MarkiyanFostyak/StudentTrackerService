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
    public class AddCourseTest
    {
        private StudentTrackerService.StudentTrackerService stService;

        private Course math = new Course { CourseName = "MathematicsTest", MaxNumberOfStudents = 15 };
        private Course calculus = new Course { CourseName = "CalculusTest", MaxNumberOfStudents = 10 };
        private Course discrete = new Course { CourseName = "DiscreteTest", MaxNumberOfStudents = 2 };

        private Course entry = new Course { CourseName = "AlgebraTest", MaxNumberOfStudents = 10 };
        private Course entryDublicate = new Course { CourseName = "CalculusTest", MaxNumberOfStudents = 10 };

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
                foreach (var cs in ctx.Courses.Where(x => x.CourseName == math.CourseName || x.CourseName == calculus.CourseName || x.CourseName == discrete.CourseName || x.CourseName == entry.CourseName))
                {
                    ctx.Courses.Remove(cs);
                }
                ctx.SaveChanges();
            }
        }

        [Test]
        public void TestAddSuccess()
        {
            bool result = stService.AddCourse(this.entry);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.IsTrue(result);
                Assert.AreEqual(initCount + 1, ctx.Courses.Count());
                Assert.AreEqual(entry.CourseName, ctx.Courses.OrderByDescending(x => x.Id).First().CourseName);
            }
        }

        [Test]
        public void TestAddDublicate()
        {
            bool result = stService.AddCourse(entryDublicate);
            Assert.IsFalse(result);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(initCount, ctx.Courses.Count());
                Assert.AreNotEqual(entry.CourseName, ctx.Courses.OrderByDescending(x => x.Id).First().CourseName);
            }
        }
    }
}
