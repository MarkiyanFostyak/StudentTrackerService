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
    public class ApplyForCourseTest
    {
        private StudentTrackerService.StudentTrackerService stService;

        private Student peter = new Student { FirstName = "PeterTest", LastName = "SimonsTest", Sex = Sex.Male, Age = 25 };
        private Student sarah = new Student { FirstName = "SarahTest", LastName = "ClarksTest", Sex = Sex.Female, Age = 20 };
        private Student jessy = new Student { FirstName = "JessyTest", LastName = "AlgienTest", Sex = Sex.Female, Age = 22 };

        private Course math = new Course { CourseName = "MathematicsTest", MaxNumberOfStudents = 15 };
        private Course calculus = new Course { CourseName = "CalculusTest", MaxNumberOfStudents = 10 };
        private Course discrete = new Course { CourseName = "DiscreteTest", MaxNumberOfStudents = 2 };

        [SetUp]
        public void Initialize()
        {

            stService = new StudentTrackerService.StudentTrackerService();
            using (var ctx = new StudentTrackerDbContext())
            {
                ctx.Students.Add(peter);
                ctx.Students.Add(sarah);
                ctx.Students.Add(jessy);
                ctx.SaveChanges();

                discrete.Students.Add(sarah);
                discrete.Students.Add(jessy);

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
                foreach(var st in ctx.Students.Where(x=>x.LastName==sarah.LastName || x.LastName==peter.LastName || x.LastName==jessy.LastName))
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
        public void TestApplyForCourseSuccess()
        {
            stService.ApplyForCourse(peter.Id, math.Id);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(peter.Courses.Count + 1, ctx.Students.Find(peter.Id).Courses.Count());
                Assert.AreEqual(math.Students.Count + 1, ctx.Courses.Find(math.Id).Students.Count());
                Assert.AreEqual(math.CourseName, ctx.Students.Find(peter.Id).Courses.OrderByDescending(x => x.Id).First().CourseName);
                Assert.AreEqual(peter.LastName, ctx.Courses.Find(math.Id).Students.OrderByDescending(x => x.Id).First().LastName);
            }
        }

        [Test]
        public void TestApplyForCourseNoStudentId()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate {stService.ApplyForCourse(-1, math.Id);});
            using(var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(math.Students.Count(), ctx.Courses.Find(math.Id).Students.Count());
            }         
        }

        [Test]
        public void TestApplyForCourseNoCourseId()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate { stService.ApplyForCourse(peter.Id,-1); });
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(peter.Courses.Count(), ctx.Students.Find(peter.Id).Courses.Count());
            }
        }

        [Test]
        public void TestApplyForCourseFullCourse()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate { stService.ApplyForCourse(peter.Id, discrete.Id); });
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(peter.Courses.Count(), ctx.Students.Find(peter.Id).Courses.Count());
            }
        }
    }
}
