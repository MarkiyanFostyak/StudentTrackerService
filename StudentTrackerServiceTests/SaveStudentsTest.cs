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
    public class SaveStudentsTest
    {
        private StudentTrackerService.StudentTrackerService stService;
        private List<RawStudentInfo> infos;

        RawStudentInfo peter = new RawStudentInfo { FirstName = "PeterTest", LastName = "SimonsTest", Sex = "Male", Age = "25" };
        RawStudentInfo jason = new RawStudentInfo { FirstName = "JasonTest", LastName = "HortTest", Sex = "Male", Age = "25" };
        RawStudentInfo mary = new RawStudentInfo { FirstName = "MaryTest", LastName = "TroyTest", Sex = "Female", Age = "25" };
        private int initCount;

        [SetUp]
        public void Initialize()
        {
            stService = new StudentTrackerService.StudentTrackerService();
            using (var ctx = new StudentTrackerDbContext())
            {
                initCount = ctx.Students.Count();
            }

            infos = new List<RawStudentInfo> { peter, jason, mary };
        }

        [TearDown]
        public void CleanUp()
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                foreach (var st in ctx.Students.Where(x => x.LastName == peter.LastName || x.LastName == jason.LastName || x.LastName == mary.LastName))
                {
                    ctx.Students.Remove(st);
                }
                ctx.SaveChanges();
            }
        }

        [Test]
        public void TestSaveSuccess()
        {
            stService.SaveStudents(this.infos);
            using(var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(infos.Count + initCount, ctx.Students.Count());
                Assert.AreEqual(infos.Last().LastName, ctx.Students.OrderByDescending(x=>x.Id).First().LastName);
            }
            
        }


    }
}
