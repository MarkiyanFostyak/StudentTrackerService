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
    public class GetStudentsTest
    {
        private StudentTrackerService.StudentTrackerService stService;
        private Student peter = new Student { FirstName = "PeterTest", LastName = "SimonsTest", Sex = Sex.Male, Age = 25 };
        private Student sarah = new Student { FirstName = "SarahTest", LastName = "ClarksTest", Sex = Sex.Female, Age = 20 };
        private Student jessy = new Student { FirstName = "JessyTest", LastName = "AlgienTest", Sex = Sex.Female, Age = 22 };
        private List<StudentInfo> expectedStudents;

        [SetUp]
        public void Initialize()
        {
            stService = new StudentTrackerService.StudentTrackerService();
            expectedStudents = new List<StudentInfo>();
            using (var ctx = new StudentTrackerDbContext())
            {
                ctx.Students.Add(peter);
                ctx.Students.Add(sarah);
                ctx.Students.Add(jessy);
                ctx.SaveChanges();
                foreach (var student in ctx.Students)
                {
                    expectedStudents.Add(new StudentInfo { Id = student.Id, FirstName = student.FirstName, LastName = student.LastName, Age = student.Age, Sex = student.Sex.ToString()});
                }
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
        public void TestGetStudents()
        {
            var students = stService.GetStudents();
            Assert.IsTrue(expectedStudents.SequenceEqual(students));
        }
    }
}
