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
    public class UpdateStudentTest
    {
        private StudentTrackerService.StudentTrackerService stService;
        private Student peter = new Student { FirstName = "PeterTest", LastName = "SimonsTest", Sex = Sex.Male, Age = 25 };
        private Student sarah = new Student { FirstName = "SarahTest", LastName = "ClarksTest", Sex = Sex.Female, Age = 20 };
        private Student jessy = new Student { FirstName = "JessyTest", LastName = "AlgienTest", Sex = Sex.Female, Age = 22 };

        private string firstName = "PeteTest";
        private string lastName = "SimonosTest";
        private string sex = "Male";
        private string age = "24";

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
            }
        }

        [TearDown]
        public void CleanUp()
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                foreach (var st in ctx.Students.Where(x => x.LastName == sarah.LastName || x.LastName == peter.LastName || x.LastName == jessy.LastName || x.LastName == lastName))
                {
                    ctx.Students.Remove(st);
                }
                ctx.SaveChanges();
            }
        }

        [Test]
        public void TestSuccessUpdate()
        {
            stService.UpdateStudent(peter.Id, firstName, lastName, age, sex);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(firstName, ctx.Students.Find(peter.Id).FirstName);
                Assert.AreEqual(lastName, ctx.Students.Find(peter.Id).LastName);
            }
        }

        [Test]
        public void TestNoIdUpdate()
        {
            stService.UpdateStudent(-1, firstName, lastName, age, sex);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreNotEqual(firstName, ctx.Students.Find(peter.Id).FirstName);
                Assert.AreEqual(peter.LastName, ctx.Students.Find(peter.Id).LastName);
            }
        }

    }
}
