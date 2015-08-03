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
    public class UpdateCourseTest
    {
        private StudentTrackerService.StudentTrackerService stService;
        private int notExistingId = -1;
        private string nameToUpdate = "Mathematical AnalysysTest";
        private string capacityToUpdate = "10";
        private Course math = new Course { CourseName = "MathematicsTest", MaxNumberOfStudents = 15 };
        private Course calculus = new Course { CourseName = "CalculusTest", MaxNumberOfStudents = 10 };
        private Course discrete = new Course { CourseName = "DiscreteTest", MaxNumberOfStudents = 25 };
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
            }
        }

        [TearDown]
        public void CleanUp()
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                foreach (var cs in ctx.Courses.Where(x => x.CourseName == math.CourseName || x.CourseName == discrete.CourseName || x.CourseName == calculus.CourseName || x.CourseName == nameToUpdate))
                {
                    ctx.Courses.Remove(cs);
                }
                ctx.SaveChanges();
            }
        }

        [Test]
        public void TestUpdateCourseSuccess()
        {
            bool result = stService.UpdateCourse(this.math.Id.ToString(), this.nameToUpdate, this.capacityToUpdate);
            Assert.IsTrue(result);
            using(var ctx= new StudentTrackerDbContext())
            {
                Assert.AreEqual(nameToUpdate, ctx.Courses.Single(x => x.Id == math.Id).CourseName);
            }          
        }

        [Test]
        public void TestUpdateCourseIdDoesNotExist()
        {
            bool result = stService.UpdateCourse(this.notExistingId.ToString(), this.nameToUpdate, this.capacityToUpdate);
            Assert.IsFalse(result);
            using(var ctx = new StudentTrackerDbContext())
            {
                Assert.IsFalse(ctx.Courses.Any(x => x.CourseName == nameToUpdate));
            }  
        }

        [Test]
        public void TestUpdateCourseDublicateName()
        {
            bool result = stService.UpdateCourse(math.Id.ToString(), discrete.CourseName, this.capacityToUpdate);
            Assert.IsFalse(result);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreNotEqual(ctx.Courses.Single(x => x.Id == math.Id), discrete.CourseName);
            }

        }
    }
}
