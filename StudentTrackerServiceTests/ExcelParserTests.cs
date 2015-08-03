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
    public class ExcelParserTests
    {
        private StudentTrackerService.StudentTrackerService stService;

        [SetUp]
        public void Initialize()
        {
            stService = new StudentTrackerService.StudentTrackerService();

            using (var ctx = new StudentTrackerDbContext())
            {
                ctx.Students.Add(new Student { FirstName = "DoubleName", LastName = "DoubleName", Age = 20, Sex = Sex.Male });
                ctx.SaveChanges();
            }
        }

        [TearDown]
        public void CleanUp()
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                var s = ctx.Students.Where(x => x.FirstName == "DoubleName" && x.LastName == "DoubleName").ToList();
                foreach (var st in ctx.Students.Where(x => x.FirstName == "DoubleName" && x.LastName == "DoubleName"))
                {
                    ctx.Students.Remove(st);
                }  
                ctx.SaveChanges();
            }
        }

        [Test]
        public void TestValidateAllValidRecords()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="John", LastName="Conn", Age="20", Sex = "Male"},
                new RawStudentInfo{FirstName="Sarah", LastName="Libouee", Age="25", Sex = "Female"},
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            Assert.IsTrue(records.SequenceEqual(stService.GetRecords(records, "En-US").ValidRecords));          
        }

        [Test]
        public void TestValidateHaveOneWithEmptyFirstName()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="", LastName="Conn", Age="20", Sex = "Male"},
                new RawStudentInfo{FirstName="Sarah", LastName="Libouee", Age="25", Sex = "Female"},
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="Sarah", LastName="Libouee", Age="25", Sex = "Female"},
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="", LastName="Conn", Age="20", Sex = "Male", Comment="1) First Name is empty. "}
            };

            List<InvalidRecord> expectedInvalidUk = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="", LastName="Conn", Age="20", Sex = "Male", Comment="1) Ім'я порожнє. "}
            };

           
            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult expextedResultUk = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalidUk };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "en-us");
            ExcelDocumentImportResult actualResultUk = stService.GetRecords(records, "uk-ua");
            Assert.AreEqual(expextedResult, actualResult);
            Assert.AreEqual(expextedResultUk, actualResultUk);  
        }

        [Test]
        public void TestValidateHaveOneWithEmptyFirstNameUk()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="", LastName="Conn", Age="20", Sex = "Male"},
                new RawStudentInfo{FirstName="Sarah", LastName="Libouee", Age="25", Sex = "Female"},
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="Sarah", LastName="Libouee", Age="25", Sex = "Female"},
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<InvalidRecord> expectedInvalidUk = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="", LastName="Conn", Age="20", Sex = "Male", Comment="1) Ім'я порожнє. "}
            };

            ExcelDocumentImportResult expextedResultUk = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalidUk };
            ExcelDocumentImportResult actualResultUk = stService.GetRecords(records, "uk-ua");
            Assert.AreEqual(expextedResultUk, actualResultUk);
        }

        [Test]
        public void TestValidateEmptyNames()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="", LastName="", Age="20", Sex = "Male"},
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
               new InvalidRecord{FirstName="", LastName="", Age="20", Sex = "Male", Comment = "1) First Name is empty. 2) Last Name is empty. " }
            };

            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "en-us");
            Assert.AreEqual(expextedResult, actualResult);  
        }

        [Test]
        public void TestValidateEmptyNamesUk()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="", LastName="", Age="20", Sex = "Male"},
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                
                 new InvalidRecord{FirstName="", LastName="", Age="20", Sex = "Male", Comment = "1) Ім'я порожнє. 2) Прізвище порожнє. " }
            };

            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "uk-ua");
            Assert.AreEqual(expextedResult, actualResult);
        }

        [Test]
        public void TestValidateInvalidChars()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="Sarah9", LastName="Libouee)(", Age="25", Sex = "Female"},
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="Sarah9", LastName="Libouee)(", Age="25", Sex = "Female", Comment = "1) First name contains restricted symbols. 2) Last name contains restricted symbols. " }
            };

            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "en-us");
            Assert.AreEqual(expextedResult, actualResult);
        }

        [Test]
        public void TestValidateInvalidCharsUk()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="Sarah9", LastName="Libouee)(", Age="25", Sex = "Female"},
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi", Age="22", Sex = "Female"}
            };

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="Sarah9", LastName="Libouee)(", Age="25", Sex = "Female", Comment = "1) Ім'я містить недозволені символи. 2) Прізвище містить недозволені символи. " }
            };

            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "uk-ua");
            Assert.AreEqual(expextedResult, actualResult);
        }

        [Test]
        public void TestValidateWrongSexAndInvalidChars()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="John", LastName="Conn", Age="20", Sex = "MALe"},
                new RawStudentInfo{FirstName="Sarah", LastName="Libouee", Age="25", Sex = "Famale"},
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi0", Age="22", Sex = "Female"}
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>
            {
                 new RawStudentInfo{FirstName="John", LastName="Conn", Age="20", Sex = "Male"}
            };

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="Sarah", LastName="Libouee", Age="25", Sex = "Famale", Comment="1) Sex is not in valid format. "},
                new InvalidRecord{FirstName="Lucia", LastName="Fabi0", Age="22", Sex = "Female", Comment = "1) Last name contains restricted symbols. "}
            };

            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "en-us");
            Assert.AreEqual(expextedResult, actualResult);  
        }

        [Test]
        public void TestValidateWrongSexAndInvalidCharsUk()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="John", LastName="Conn", Age="20", Sex = "MALe"},
                new RawStudentInfo{FirstName="Sarah", LastName="Libouee", Age="25", Sex = "Famale"},
                new RawStudentInfo{FirstName="Lucia", LastName="Fabi0", Age="22", Sex = "Female"}
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>
            {
                 new RawStudentInfo{FirstName="John", LastName="Conn", Age="20", Sex = "Male"}
            };

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="Sarah", LastName="Libouee", Age="25", Sex = "Famale", Comment="1) Неправильний формат статі. "},
                new InvalidRecord{FirstName="Lucia", LastName="Fabi0", Age="22", Sex = "Female", Comment = "1) Прізвище містить недозволені символи. "}
            };

            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "uk-ua");
            Assert.AreEqual(expextedResult, actualResult);
        }

        [Test]
        public void TestValidateWrongFormatOfAge()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="John", LastName="Conn", Age="Twenty", Sex = "MALe"},
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>();
            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="John", LastName="Conn", Age="Twenty", Sex = "MALe", Comment="1) Invalid format of Age. "}
            };
            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "en-us");
            Assert.AreEqual(expextedResult, actualResult);  
        }

        [Test]
        public void TestValidateWrongFormatOfAgeUk()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="John", LastName="Conn", Age="Twenty", Sex = "MALe"},
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>();
            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="John", LastName="Conn", Age="Twenty", Sex = "MALe", Comment="1) Неправильний формат віку. "}
            };
            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "uk-ua");
            Assert.AreEqual(expextedResult, actualResult);
        }

        [Test]
        public void TestValidateDublicateNameInDoc()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="John", LastName="Conn", Age="18", Sex = "Male"},
                new RawStudentInfo{FirstName="John", LastName="Conn", Age="25", Sex = "Male"}
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="John", LastName="Conn", Age="18", Sex = "Male"}
            };

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="John", LastName="Conn", Age="25", Sex = "Male", Comment="1) Dublicate name: [John Conn]. "}
            };

            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "en-us");
            Assert.AreEqual(expextedResult, actualResult);
        }

        [Test]
        public void TestValidateDublicateNameInDocUk()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="John", LastName="Conn", Age="18", Sex = "Male"},
                new RawStudentInfo{FirstName="John", LastName="Conn", Age="25", Sex = "Male"}
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="John", LastName="Conn", Age="18", Sex = "Male"}
            };

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="John", LastName="Conn", Age="25", Sex = "Male", Comment="1) Повтор імені: [John Conn]. "}
            };

            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "uk-ua");
            Assert.AreEqual(expextedResult, actualResult);
        }

        [Test]
        public void TestValidateDublicateNameInDb()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="DoubleName", LastName="DoubleName", Age="20", Sex = "Male"},
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>();

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="DoubleName", LastName="DoubleName", Age="20", Sex = "Male", Comment="1) Dublicate name: [DoubleName DoubleName]. "}
            };

            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };

            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "en-us");
            Assert.AreEqual(expextedResult, actualResult);
        }

        [Test]
        public void TestValidateDublicateNameInDbUk()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="DoubleName", LastName="DoubleName", Age="20", Sex = "Male"},
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo>();

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>
            {
                new InvalidRecord{FirstName="DoubleName", LastName="DoubleName", Age="20", Sex = "Male", Comment="1) Повтор імені: [DoubleName DoubleName]. "}
            };

            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };

            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "uk-ua");
            Assert.AreEqual(expextedResult, actualResult);
        }

        [Test]
        public void TestValidateFormatName()
        {
            List<RawStudentInfo> records = new List<RawStudentInfo>
            {
                new RawStudentInfo{FirstName="PaUl", LastName="FRAnkliN", Age="19", Sex = "Male"},
            };

            List<RawStudentInfo> expectedValid = new List<RawStudentInfo> 
            { 
                new RawStudentInfo{FirstName="Paul", LastName="Franklin", Age="19", Sex="Male"}
            };

            List<InvalidRecord> expectedInvalid = new List<InvalidRecord>();
          

            ExcelDocumentImportResult expextedResult = new ExcelDocumentImportResult { ValidRecords = expectedValid, InvalidRecords = expectedInvalid };
            ExcelDocumentImportResult actualResult = stService.GetRecords(records, "en-us");
            Assert.AreEqual(expextedResult, actualResult);
        }

    }
}
