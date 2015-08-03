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
    public class GetStudentTest
    {
        private StudentTrackerService.StudentTrackerService stService;
        private Student peter = new Student { FirstName = "PeterTest", LastName = "SimonsTest", Sex = Sex.Male, Age = 25 };
        int id;

        [SetUp]
        public void Initialize()
        {
            stService = new StudentTrackerService.StudentTrackerService();
            using (var ctx = new StudentTrackerDbContext())
            {
                ctx.Students.Add(peter);
                ctx.SaveChanges();

                id = ctx.Students.Single(x => x.FirstName == peter.FirstName && x.LastName == peter.LastName && x.Age == peter.Age).Id;
            }
        }

        [TearDown]
        public void CleanUp()
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                foreach (var st in ctx.Students.Where(x => x.LastName == peter.LastName))
                {
                    ctx.Students.Remove(st);
                }
                ctx.SaveChanges();
            }
        }

        [Test]
        public void TestGetSuccess()
        {
            var student = stService.GetStudent(id);
            Assert.IsNotNull(student);
            Assert.AreEqual(peter.FirstName, student.FirstName);
            Assert.AreEqual(peter.LastName, student.LastName);
        }

        [Test]
        public void TestGetNoId()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate { stService.GetStudent(-1); });
        }
    }
}
