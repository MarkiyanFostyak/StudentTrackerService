using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using StudentTracker.Domain;
using StudentTracker.Domain.Concrete;
using StudentTracker.Domain.Entities;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OfficeOpenXml;
using StudentTracker.Domain.IdentityEntities;

namespace StudentTrackerService
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class StudentTrackerService : ICoursesManager, IStudentsManager
    {

        #region CourseManager

        public List<CourseInfo> GetCourses()
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                List<CourseInfo> courses = new List<CourseInfo>();
                foreach (Course course in ctx.Courses)
                {
                    courses.Add(new CourseInfo
                    {
                        CourseName = course.CourseName,
                        CurrentlyAssigned = course.Students.Count,
                        Id = course.Id,
                        MaxAssigned = course.MaxNumberOfStudents
                    });
                }
                return courses;
            }
        }

        public void DeleteCourse(string courseId)
        {
            int id;
            if (int.TryParse(courseId, out id))
            {
                using (var ctx = new StudentTrackerDbContext())
                {
                    var course = ctx.Courses.Find(id);
                    if (course != null)
                    {
                        ctx.Courses.Remove(course);
                        ctx.SaveChanges();
                    }
                }
            }
        }

        public bool AddCourse(Course entity)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                if (ctx.Courses.Any(x => x.CourseName.Equals(entity.CourseName, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
                ctx.Courses.Add(new Course { CourseName = entity.CourseName, MaxNumberOfStudents = entity.MaxNumberOfStudents });
                ctx.SaveChanges();
                var course = ctx.Courses.OrderByDescending(x => x.Id).First();
                entity.Id = course != null ? course.Id : 0;
                return true;
            }
        }

        public bool UpdateCourse(string id, string name, string capacity)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                int courseId = int.Parse(id);
                if (ctx.Courses.Any(x => x.CourseName.Equals(name, StringComparison.OrdinalIgnoreCase) && x.Id != courseId))
                {
                    return false;
                }
                else
                {
                    var course = ctx.Courses.Find(courseId);
                    if (course != null)
                    {
                        course.CourseName = name;
                        course.MaxNumberOfStudents = int.Parse(capacity);
                        ctx.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
        }

        #endregion

        #region StudentManager

        public StudentInfo GetStudent(int id)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                var student =  ctx.Students.Find(id);
                if (student == null)
                {
                    throw new InvalidOperationException("No studnet with the id specified found");
                }
                return new StudentInfo { Id = student.Id, FirstName = student.FirstName, LastName = student.LastName, Age = student.Age, Sex = student.Sex.ToString() };
            }
        }

        public bool AddStudent(Student entry)
        {
            using (var ctx = new StudentTrackerDbContext())
            {            
                if (IsDublicateName(entry.FirstName, entry.LastName, ctx.Students))
                {
                    return false; 
                }
                ctx.Students.Add(entry);
                ctx.SaveChanges();
                var student = ctx.Students.OrderByDescending(x => x.Id).First();
                entry.Id = student != null ? student.Id : 0;
                return true;
            }
        }

        public List<StudentInfo> GetStudents()
        {
            List<StudentInfo> students = new List<StudentInfo>();
            using (var ctx = new StudentTrackerDbContext())
            {
                foreach (Student student in ctx.Students)
                {
                    List<string> courses = student.Courses.Select(x => x.CourseName).ToList();
                    students.Add(new StudentInfo
                    {
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Age = student.Age,
                        Id = student.Id,
                        Sex = student.Sex.ToString(),
                        Courses = courses
                    });
                }
            }
            return students;
        }

        public void UpdateStudent(int id, string firstName, string lastName, string age, string sex)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                var student = ctx.Students.Find(id);
                if (student == null)
                {
                    return;
                }
                student.FirstName = firstName;
                student.LastName = lastName;
                student.Age = int.Parse(age);
                student.Sex = sex.Equals("Female") ? Sex.Female : Sex.Male;
                ctx.SaveChanges();
            }
        }

        public void DeleteStudent(int id)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                var st = ctx.Students.Find(id);
                if (st != null)
                {
                    ctx.Students.Remove(st);
                    ctx.SaveChanges();
                }
            }
        }

        public string GetStudentFullName(int studentId)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                var st = ctx.Students.Find(studentId);
                if (st == null)
                {
                    throw new InvalidOperationException("No studnet with the id specified found");
                }
                return st.FirstName + " " + st.LastName;
            }
        }

        public string GetStudentsSex(int studentId)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                var st = ctx.Students.Find(studentId);
                if (st == null)
                {
                    throw new InvalidOperationException("No studnet with the id specified found");
                }
                return st.Sex.ToString();
            }
        }

        public void SaveStudents(IEnumerable<RawStudentInfo> students)
        {
            foreach (var rawStudent in students)
            {
                AddStudent(rawStudent.ToStudent());
            }
        }

        #endregion

        #region StudentsCourses

        public List<StudentCourse> GetStudentsCourses(int studentId)
        {
            List<StudentCourse> courses = new List<StudentCourse>();
            using (var ctx = new StudentTrackerDbContext())
            {
                if (ctx.Students.Find(studentId) == null)
                {
                    throw new InvalidOperationException("Student with the specified id does not exist");
                }
                foreach (var course in ctx.Courses)
                {
                    bool isApplied = (course.Students.SingleOrDefault(x => x.Id == studentId)) != null;
                    courses.Add(new StudentCourse
                    {
                        Id = course.Id,
                        CourseName = course.CourseName,
                        IsAppliedFor = isApplied,
                        IsActive = isApplied || course.Students.Count < course.MaxNumberOfStudents,
                        AssignedStudents = course.Students.Count,
                        MaxStudents = course.MaxNumberOfStudents
                    });
                }
            }
            return courses;
        }

        public void ApplyForCourse(int studentId, int courseId)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                var student = ctx.Students.Find(studentId);
                var course = ctx.Courses.Find(courseId);
                if (student == null)
                {
                    throw new InvalidOperationException("No student with specified id found.");
                }
                if (course == null)
                {
                    throw new InvalidOperationException("No course with specified id found.");
                }
                if (course.Students.Count==course.MaxNumberOfStudents)
                {
                    throw new InvalidOperationException("Course is full.");
                }
                course.Students.Add(student);
                ctx.SaveChanges();
            }
        }

        public void LeaveCourse(int studentId, int courseId)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                Student student = ctx.Students.Find(studentId);
                Course course = ctx.Courses.Find(courseId);
                if (student == null)
                {
                    throw new InvalidOperationException("Student with specified id does not exist.");
                }
                if (course == null)
                {
                    throw new InvalidOperationException("Course with specified id does not exist.");
                }
                course.Students.Remove(student);
                ctx.SaveChanges();
            }
        }

        #endregion

        #region ExcelParser
        public ExcelDocumentImportResult GetRecords(IEnumerable<RawStudentInfo> studentsRawInformation, string culture)
        {
            List<RawStudentInfo> validRecords = new List<RawStudentInfo>();
            List<InvalidRecord> invalidRecords = new List<InvalidRecord>();
            foreach (RawStudentInfo studentInformation in studentsRawInformation)
            {
                Validate(studentInformation, ref validRecords, ref invalidRecords, culture);
            }
            return new ExcelDocumentImportResult { ValidRecords = validRecords, InvalidRecords = invalidRecords };
        }

        private void Validate(RawStudentInfo rowInfo, ref List<RawStudentInfo> validRecords, ref List<InvalidRecord> invalidRecords, string culture)
        {
            bool isValid = true;
            int number;
            string comment = string.Empty;
            int i = 1;
            if (string.IsNullOrEmpty(rowInfo.FirstName) || string.IsNullOrWhiteSpace(rowInfo.FirstName))
            {
                isValid = false;
                string commentPoint = culture.Equals("uk-ua", StringComparison.InvariantCultureIgnoreCase) ? LocalizationResources.EmptyFirstNameUk : LocalizationResources.EmptyFirstName;
                comment += string.Format("{0}) {1} ", i++, commentPoint);
            }
            if (string.IsNullOrEmpty(rowInfo.LastName) || string.IsNullOrWhiteSpace(rowInfo.LastName))
            {
                isValid = false;
                string commentPoint = culture.Equals("uk-ua", StringComparison.InvariantCultureIgnoreCase) ? LocalizationResources.EmptyLastNameUk : LocalizationResources.EmptyLastName;
                comment += String.Format("{0}) {1} ", i++, commentPoint);
            }
            if (!rowInfo.Sex.Equals("Male", StringComparison.OrdinalIgnoreCase) && !rowInfo.Sex.Equals("Female", StringComparison.OrdinalIgnoreCase))
            {
                isValid = false;
                string commentPoint = culture.Equals("uk-ua", StringComparison.InvariantCultureIgnoreCase) ? LocalizationResources.InvalidSexUk : LocalizationResources.InvalidSex;
                comment += string.Format("{0}) {1} ", i++, commentPoint);
            }
            if (!int.TryParse(rowInfo.Age, out number))
            {
                isValid = false;
                string commentPoint = culture.Equals("uk-ua", StringComparison.InvariantCultureIgnoreCase) ? LocalizationResources.InvalidAgeUk : LocalizationResources.InvalidAge;
                comment += String.Format("{0}) {1} ", i++, commentPoint);
            }

            if (!Regex.IsMatch(rowInfo.FirstName, @"^[a-zA-Z]*$"))
            {
                isValid = false;
                string commentPoint = culture.Equals("uk-ua", StringComparison.InvariantCultureIgnoreCase) ? LocalizationResources.InvalidFirstNameUk : LocalizationResources.InvalidFirstName;
                comment += String.Format("{0}) {1} ", i++, commentPoint);
            }
            if (!Regex.IsMatch(rowInfo.LastName, @"^[a-zA-Z]*$"))
            {
                isValid = false;
                string commentPoint = culture.Equals("uk-ua", StringComparison.InvariantCultureIgnoreCase) ? LocalizationResources.InvalidLastNameUk : LocalizationResources.InvalidLastName;
                comment += String.Format("{0}) {1} ", i++, commentPoint);
            }
            if (IsDublicateName(rowInfo.FirstName, rowInfo.LastName, validRecords))
            {
                isValid = false;
                string commentPoint = culture.Equals("uk-ua", StringComparison.InvariantCultureIgnoreCase) ? LocalizationResources.DublicateNameUk : LocalizationResources.DublicateName;
                comment += String.Format("{0}) {3} [{1} {2}]. ", i, rowInfo.FirstName, rowInfo.LastName, commentPoint);
            }
            if (isValid)
            {
                using (var ctx = new StudentTrackerDbContext())
                {
                    if (IsDublicateName(rowInfo.FirstName, rowInfo.LastName, ctx.Students))
                    {
                        isValid = false;
                        string commentPoint = culture.Equals("uk-ua", StringComparison.InvariantCultureIgnoreCase) ? LocalizationResources.DublicateNameUk : LocalizationResources.DublicateName;
                        comment += String.Format("{0}) {3} [{1} {2}]. ", i, rowInfo.FirstName, rowInfo.LastName, commentPoint);
                    }
                }
            }
            if (isValid)
            {
                rowInfo.FirstName = FormatName(rowInfo.FirstName);
                rowInfo.LastName = FormatName(rowInfo.LastName);
                rowInfo.Sex = FormatName(rowInfo.Sex);
                validRecords.Add(rowInfo);
            }
            else
            {
                invalidRecords.Add(rowInfo.ToInvalidRecord(comment));
            }
        }

        private bool IsDublicateName(string firstName, string lastName, IEnumerable<Student> students)
        {
            return
                students.Any(
                    x =>
                        x.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                        x.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsDublicateName(string firstName, string lastName, IEnumerable<RawStudentInfo> students)
        {
            return
                students.Any(
                    x =>
                        x.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                        x.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));
        }

        private string FormatName(string name)
        {
            char[] nameChars = name.ToLower().ToCharArray();
            nameChars[0] = char.ToUpper(nameChars[0]);
            return new string(nameChars);
        }

        #endregion

        #region UiSettings
        public UiSettings GetUiColours(string id)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                UiSettings settings = new UiSettings { BackgroundColour = "#ffffff", HeaderColour = "#101010" };
                var user = ctx.Users.Find(id);
                if (user == null)
                {
                    throw new InvalidOperationException("No user with specified id found.");
                }
                var uiColour = user.UiColour;
                if (uiColour != null)
                {
                    settings.BackgroundColour = string.IsNullOrEmpty(uiColour.BackgroundColour) ? "#ffffff" : uiColour.BackgroundColour;
                    settings.HeaderColour = string.IsNullOrEmpty(uiColour.HeaderColour) ? "#101010" : uiColour.HeaderColour;
                }
                return settings;
            }
        }

        public UiSettings GetUiColoursByName(string name)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                UiSettings settings = new UiSettings { BackgroundColour = "#ffffff", HeaderColour = "#101010" };
                
                if (!ctx.Users.Any(x=>x.UserName==name))
                {
                    throw new InvalidOperationException("No user with specified name found.");
                }
                else
                {
                    var user = ctx.Users.Single(x=>x.UserName==name);
                    var uiColour = user.UiColour;
                    if (uiColour != null)
                    {
                        settings.BackgroundColour = string.IsNullOrEmpty(uiColour.BackgroundColour) ? "#ffffff" : uiColour.BackgroundColour;
                        settings.HeaderColour = string.IsNullOrEmpty(uiColour.HeaderColour) ? "#101010" : uiColour.HeaderColour;
                    }
                }            
                return settings;
            }
        }

        public void SetUiColours(string id, string backgroundColour, string headerColour)
        {
            using (var ctx = new StudentTrackerDbContext())
            {
                var user = ctx.Users.Find(id);
                if (user == null)
                {
                    throw new InvalidOperationException("No user with specified name found.");
                }
                if (ctx.Users.Single(x => x.Id == id).UiColour == null)
                {
                    ctx.Users.Single(x => x.Id == id).UiColour = new UiColour { HeaderColour = headerColour, BackgroundColour = backgroundColour };
                }
                else
                {
                    ctx.Users.Single(x => x.Id == id).UiColour.BackgroundColour = backgroundColour;
                    ctx.Users.Single(x => x.Id == id).UiColour.HeaderColour = headerColour;
                }
                ctx.SaveChanges();
            }
        }

        #endregion
    }
}
