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
    public class DeleteStudentTest
    {
        private StudentTrackerService.StudentTrackerService stService;
        private Student peter = new Student { FirstName = "PeterTest", LastName = "SimonsTest", Sex = Sex.Male, Age = 25 };
        private Student sarah = new Student { FirstName = "SarahTest", LastName = "ClarksTest", Sex = Sex.Female, Age = 20 };
        private Student jessy = new Student { FirstName = "JessyTest", LastName = "AlgienTest", Sex = Sex.Female, Age = 22 };

        private int initCount;

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
                initCount = ctx.Students.Count();
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
                ctx.SaveChanges();
            }
        }

        [Test]
        public void TestDeleteSuccess()
        {
            stService.DeleteStudent(peter.Id);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(initCount - 1, ctx.Students.Count());
                Assert.IsNull(ctx.Students.Find(peter.Id));
            }
        }

        [Test]
        public void TestDeleteNoId()
        {
            stService.DeleteStudent(-1);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(initCount, ctx.Students.Count());
            }
        }

    }
}
