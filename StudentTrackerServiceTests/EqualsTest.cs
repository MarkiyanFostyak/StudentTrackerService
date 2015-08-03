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
    public class EqualsTest
    {
        private CourseInfo courseInfoBase = new CourseInfo { Id = 1, CourseName = "Math", CurrentlyAssigned = 10, MaxAssigned = 15 };
        private CourseInfo courseInfoEqual = new CourseInfo { Id = 1, CourseName = "Math", CurrentlyAssigned = 10, MaxAssigned = 15 };
        private CourseInfo courseInfoNotEqual = new CourseInfo { Id = 3, CourseName = "Chemistry", CurrentlyAssigned = 15, MaxAssigned = 15 };

        private InvalidRecord invalidRecordBase = new InvalidRecord { FirstName = "Alex", LastName = "Bell", Sex = "Male", Age = "25", Comment = "1) First Name is empty." };
        private InvalidRecord invalidRecordEqual = new InvalidRecord { FirstName = "Alex", LastName = "Bell", Sex = "Male", Age = "25", Comment = "1) First Name is empty." };
        private InvalidRecord invalidRecordNotEqual = new InvalidRecord { FirstName = "Sarah", LastName = "Connor", Sex = "Female", Age = "22", Comment = "1) Age is empty." };

        private StudentCourse studentCourseBase = new StudentCourse { Id = 5, CourseName = "Math", AssignedStudents = 5, MaxStudents = 10, IsActive = true, IsAppliedFor = true };
        private StudentCourse studentCourseEqual = new StudentCourse { Id = 5, CourseName = "Math", AssignedStudents = 5, MaxStudents = 10, IsActive = true, IsAppliedFor = true };
        private StudentCourse studentCourseNotEqual = new StudentCourse { Id = 6, CourseName = "Literature", AssignedStudents = 10, MaxStudents = 10, IsActive = false, IsAppliedFor = false };

        private StudentInfo studentInfoBase = new StudentInfo { Id = 1, FirstName = "Peter", LastName = "Marts", Age = 25, Sex = "Male" };
        private StudentInfo studentInfoEqual = new StudentInfo { Id = 1, FirstName = "Peter", LastName = "Marts", Age = 25, Sex = "Male" };
        private StudentInfo studentInfoNotEqual = new StudentInfo { Id = 3, FirstName = "Sarah", LastName = "Marts", Age = 20, Sex = "Female" };

        private UiSettings uiSettingsBase = new UiSettings { BackgroundColour = "#111111", HeaderColour = "#222222" };
        private UiSettings uiSettingsEqual = new UiSettings { BackgroundColour = "#111111", HeaderColour = "#222222" };
        private UiSettings uiSettingsNotEqual = new UiSettings { BackgroundColour = "#333333", HeaderColour = "#444444" };

        private RawStudentInfo rawStudentInfoBase = new RawStudentInfo { FirstName = "Marry", LastName = "Clarson", Sex = "Female", Age = "20" };
        private RawStudentInfo rawStudentInfoEqual = new RawStudentInfo { FirstName = "Marry", LastName = "Clarson", Sex = "Female", Age = "20" };
        private RawStudentInfo rawStudentInfoNotEqual = new RawStudentInfo { FirstName = "Jessy", LastName = "Frass", Sex = "Female", Age = "22" };

        private ExcelDocumentImportResult excelDocResBase = new ExcelDocumentImportResult
        {
            ValidRecords = new List<RawStudentInfo> 
            {
                new RawStudentInfo { FirstName = "Marry", LastName = "Clarson", Sex = "Female", Age = "20" }, 
                new RawStudentInfo { FirstName = "Jessy", LastName = "Frass", Sex = "Female", Age = "22" }
            },
            InvalidRecords = new List<InvalidRecord>
            {
                new InvalidRecord { FirstName = "Alex", LastName = "Bell", Sex = "Male", Age = "25", Comment = "1) First Name is empty." },
                new InvalidRecord { FirstName = "Sarah", LastName = "Connor", Sex = "Female", Age = "22", Comment = "1) Age is empty." }
            }
        };


        private ExcelDocumentImportResult excelDocResEquals = new ExcelDocumentImportResult
        {
            ValidRecords = new List<RawStudentInfo> 
            {
                new RawStudentInfo { FirstName = "Marry", LastName = "Clarson", Sex = "Female", Age = "20" }, 
                new RawStudentInfo { FirstName = "Jessy", LastName = "Frass", Sex = "Female", Age = "22" }
            },
            InvalidRecords = new List<InvalidRecord>
            {
                new InvalidRecord { FirstName = "Alex", LastName = "Bell", Sex = "Male", Age = "25", Comment = "1) First Name is empty." },
                new InvalidRecord { FirstName = "Sarah", LastName = "Connor", Sex = "Female", Age = "22", Comment = "1) Age is empty." }
            }
        };
        private ExcelDocumentImportResult excelDocResNotEquals = new ExcelDocumentImportResult
        {
            ValidRecords = new List<RawStudentInfo> 
            {
                new RawStudentInfo { FirstName = "Parker", LastName = "Clarson", Sex = "Male", Age = "20" }, 
                new RawStudentInfo { FirstName = "Jessy", LastName = "Frass", Sex = "Female", Age = "22" }
            },
            InvalidRecords = new List<InvalidRecord>
            {
                new InvalidRecord { FirstName = "Alex", LastName = "Bell", Sex = "Male", Age = "25", Comment = "1) First Name is empty." },
                new InvalidRecord { FirstName = "Sarah", LastName = "Bills", Sex = "Female", Age = "22", Comment = "1) Age is empty." }
            }
        };

        private string notEntity = "Not Entity";

        [Test]
        public void TestCourseInfoEqual()
        {
            Assert.IsTrue(courseInfoBase.Equals(courseInfoEqual));
        }

        [Test]
        public void TestCourseInfoNotEqual()
        {
            Assert.IsFalse(courseInfoBase.Equals(courseInfoNotEqual));
        }

        [Test]
        public void TestCourseInfoNotEqualObj()
        {
            Assert.IsFalse(courseInfoBase.Equals(notEntity));
        }

        [Test]
        public void TestInvalidRecordEquals()
        {
            Assert.IsTrue(invalidRecordBase.Equals(invalidRecordEqual));
        }

        [Test]
        public void TestInvalidRecordNotEqual()
        {
            Assert.IsFalse(invalidRecordBase.Equals(invalidRecordNotEqual));
        }

        [Test]
        public void TestInvalidREcordNotEqualObj()
        {
            Assert.IsFalse(invalidRecordBase.Equals(notEntity));
        }

        [Test]
        public void TestStudentCourseEqual()
        {
            Assert.IsTrue(studentCourseBase.Equals(studentCourseEqual));
        }

        [Test]
        public void TestStudentCourseNotEqual()
        {
            Assert.IsFalse(studentCourseBase.Equals(studentCourseNotEqual));
        }

        [Test]
        public void TestStudentCourseNotEqualObj() 
        {
            Assert.IsFalse(studentCourseBase.Equals(notEntity));
        }

        [Test]
        public void TestStudentInfoEquals()
        {
            Assert.IsTrue(studentInfoBase.Equals(studentInfoEqual));
        }

        [Test]
        public void TestStudentInfoNotEqual()
        {
            Assert.IsFalse(studentInfoBase.Equals(studentInfoNotEqual));
        }

        [Test]
        public void TestStudentInfoNotEqualObj()
        {
            Assert.IsFalse(studentInfoBase.Equals(notEntity));
        }

        [Test]
        public void TestUiSettingsEquals()
        {
            Assert.IsTrue(uiSettingsBase.Equals(uiSettingsEqual));
        }

        [Test]
        public void TestUiSettingsNotEqual()
        {
            Assert.IsFalse(uiSettingsBase.Equals(uiSettingsNotEqual));
        }

        [Test]
        public void TestUiSettingsNotEqualsObj()
        {
            Assert.IsFalse(uiSettingsBase.Equals(notEntity));
        }

        [Test]
        public void TestExcelDocResEquals()
        {
            Assert.IsTrue(excelDocResBase.Equals(excelDocResEquals));
        }

        [Test]
        public void TestExcelDocResNotEquals()
        {
            Assert.IsFalse(excelDocResBase.Equals(excelDocResNotEquals));
        }

        [Test]
        public void TestExcelDocResNotEqualsObj()
        {
            Assert.IsFalse(excelDocResBase.Equals(notEntity));
        }

        [Test]
        public void TestRawStudentInfoEquals()
        {
            Assert.IsTrue(rawStudentInfoBase.Equals(rawStudentInfoEqual));
        }

        [Test]
        public void TestRawStudentInfoNotEquals()
        {
            Assert.IsFalse(rawStudentInfoBase.Equals(rawStudentInfoNotEqual));
        }

        [Test]
        public void TestRawStudentInfoNotEqualsObj()
        {
            Assert.IsFalse(rawStudentInfoBase.Equals(notEntity));
        }

    }
}
