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
    public class GetStudentsCoursesTest
    {
        private StudentTrackerService.StudentTrackerService stService;

        private Student peter;
        private Student sarah;
        private Student jessy;

        private Course math;
        private Course calculus;
        private Course discrete;

        private List<StudentCourse> expectedCourses;

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

                discrete.Students.Add(sarah);
                calculus.Students.Add(sarah);

                ctx.Courses.Add(math);
                ctx.Courses.Add(calculus);
                ctx.Courses.Add(discrete);

                ctx.SaveChanges();

                expectedCourses = new List<StudentCourse>();
                foreach (var course in ctx.Courses)
                {
                    expectedCourses.Add(new StudentCourse { CourseName = course.CourseName, AssignedStudents = course.Students.Count, Id = course.Id, IsActive = course.Students.Count < course.MaxNumberOfStudents, MaxStudents = course.MaxNumberOfStudents, IsAppliedFor = course.Students.Any(x => x.Id == sarah.Id) });
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

                foreach (var cs in ctx.Courses.Where(x => x.CourseName == math.CourseName || x.CourseName == discrete.CourseName || x.CourseName == calculus.CourseName))
                {
                    ctx.Courses.Remove(cs);
                }

                ctx.SaveChanges();
            }
        }

        [Test]
        public void TestGetStudentsCoursesSuccess()
        {
            var courses = stService.GetStudentsCourses(sarah.Id);
            Assert.IsTrue(expectedCourses.SequenceEqual(courses));
        }

        [Test]
        public void TestGetStudentsCoursesNoId()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate { stService.GetStudentsCourses(-1); });
        }

        [Test]
        public void TestGetStudentsWithCourses()
        {
            var students = stService.GetStudents();
            Assert.AreEqual(2, students.Single(x => x.FirstName == sarah.FirstName).Courses.Count); 
        }
    }
}

