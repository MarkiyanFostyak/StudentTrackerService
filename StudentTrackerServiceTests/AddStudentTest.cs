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
    public class AddStudentTest
    {
        private StudentTrackerService.StudentTrackerService stService;
        private Student peter = new Student { FirstName = "PeterTest", LastName = "SimonsTest", Sex = Sex.Male, Age = 25 };
        private Student sarah = new Student { FirstName = "SarahTest", LastName = "ClarksTest", Sex = Sex.Female, Age = 20 };
        private Student jessy = new Student { FirstName = "JessyTest", LastName = "AlgienTest", Sex = Sex.Female, Age = 22 };

        private Student entry = new Student { FirstName = "NelsonTest", LastName = "GrayeeTest", Sex = Sex.Male, Age = 21 };
        private Student dublicateEntry = new Student { FirstName = "PeterTest", LastName = "SimonsTest", Sex = Sex.Male, Age = 21 };

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
                foreach (var st in ctx.Students.Where(x => x.LastName == sarah.LastName || x.LastName == peter.LastName || x.LastName == jessy.LastName || x.LastName==entry.LastName))
                {
                    ctx.Students.Remove(st);
                }
                ctx.SaveChanges();
            }
        }

        [Test]
        public void TestAddSuccess()
        {
            bool result = stService.AddStudent(entry);
            Assert.IsTrue(result);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(initCount + 1, ctx.Students.Count());
                Assert.AreEqual(entry.FirstName, ctx.Students.OrderByDescending(x => x.Id).First().FirstName);
            }
        }

        [Test]
        public void TestAddDublicateName()
        {
            bool result = stService.AddStudent(dublicateEntry);
            Assert.IsFalse(result);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(initCount, ctx.Students.Count());
                Assert.AreNotEqual(dublicateEntry.FirstName, ctx.Students.OrderByDescending(x => x.Id).First().FirstName);
            }
        }
    }
}
