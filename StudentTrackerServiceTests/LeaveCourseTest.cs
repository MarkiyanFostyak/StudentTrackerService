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
    public class LeaveCourseTest
    {
        private StudentTrackerService.StudentTrackerService stService;

        private Student peter;
        private Student sarah;
        private Student jessy;

        private Course math;
        private Course calculus;
        private Course discrete;

        [SetUp]
        public void Initialize()
        {
            stService = new StudentTrackerService.StudentTrackerService();
            peter = new Student { FirstName = "PeterTest", LastName = "SimonsTest", Sex = Sex.Male, Age = 25 };
            sarah = new Student { FirstName = "SarahTest", LastName = "ClarksTest", Sex = Sex.Female, Age = 20 };
            jessy = new Student { FirstName = "JessyTest", LastName = "AlgienTest", Sex = Sex.Female, Age = 22 };

            math = new Course { CourseName = "MathematicsTest", MaxNumberOfStudents = 15 };
            calculus = new Course { CourseName = "CalculusTest", MaxNumberOfStudents = 10 };
            discrete = new Course { CourseName = "DiscreteTest", MaxNumberOfStudents = 2 };

            using (var ctx = new StudentTrackerDbContext())
            {
                ctx.Students.Add(peter);
                ctx.Students.Add(sarah);
                ctx.Students.Add(jessy);
                ctx.SaveChanges();

                math.Students.Add(peter);
                math.Students.Add(jessy);

                ctx.Courses.Add(math);
                ctx.Courses.Add(calculus);
                ctx.Courses.Add(discrete);
               
                ctx.SaveChanges();
            }
        }

        [TearDown]
        public void CleanUp()
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                foreach (var st in ctx.Students.Where(x => x.LastName == sarah.LastName || x.LastName == peter.LastName || x.LastName == jessy.LastName))
                {
                    ctx.Students.Remove(st);
                }

                foreach (var cs in ctx.Courses.Where(x => x.CourseName == math.CourseName || x.CourseName == discrete.CourseName || x.CourseName == calculus.CourseName))
                {
                    ctx.Courses.Remove(cs);
                }

                ctx.SaveChanges();
            }
        }

        [Test]
        public void TestLeaveCourseSuccess()
        {
            stService.LeaveCourse(peter.Id, math.Id);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(peter.Courses.Count - 1, ctx.Students.Find(peter.Id).Courses.Count());
                Assert.AreEqual(math.Students.Count - 1, ctx.Courses.Find(math.Id).Students.Count());
                Assert.IsFalse(ctx.Students.Find(peter.Id).Courses.Any(x=>x.Id==math.Id));
                Assert.IsFalse(ctx.Courses.Find(math.Id).Students.Any(x=>x.Id==peter.Id));
            }
        }

        [Test]
        public void TestLeaveCourseNoStudentId()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate {stService.LeaveCourse(-1, math.Id);});
            using(var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(math.Students.Count(), ctx.Courses.Find(math.Id).Students.Count());
            }         
        }

        [Test]
        public void TestLeaveCourseNoCourseId()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate { stService.LeaveCourse(peter.Id,-1); });
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(peter.Courses.Count(), ctx.Students.Find(peter.Id).Courses.Count());
            }
        }
    }
}
