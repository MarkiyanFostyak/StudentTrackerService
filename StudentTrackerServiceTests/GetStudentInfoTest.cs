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
    public class GetStudentInfoTest
    {
        private StudentTrackerService.StudentTrackerService stService;
        private Student peter = new Student { FirstName = "PeterTest", LastName = "SimonsTest", Sex = Sex.Male, Age = 25 };
        private Student sarah = new Student { FirstName = "SarahTest", LastName = "ClarksTest", Sex = Sex.Female, Age = 20 };
        private Student jessy = new Student { FirstName = "JessyTest", LastName = "AlgienTest", Sex = Sex.Female, Age = 22 };

        private string expectedFullName;
        private string expectedSex;
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
            expectedFullName = string.Format("{0} {1}", peter.FirstName, peter.LastName);
            expectedSex = sarah.Sex.ToString();
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
        public void TestGetFullNameSuccess()
        {
            Assert.AreEqual(expectedFullName, stService.GetStudentFullName(peter.Id));
        }

        [Test]
        public void TestGetFullNameNoId()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate { stService.GetStudentFullName(-1); });
        }

        [Test]
        public void TestGetSexSuccess()
        {
            Assert.AreEqual(expectedSex, stService.GetStudentsSex(sarah.Id));
        }

        [Test]
        public void TestGetSexNoId()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate { stService.GetStudentsSex(-1); });
        }

    }
}
