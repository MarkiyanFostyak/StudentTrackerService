using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IO;
using OfficeOpenXml;
using System.Globalization;
using StudentTracker.Domain.Entities;

namespace StudentTrackerService
{
    [ServiceContract]
    public interface IStudentsManager
    {
        [OperationContract]
        void SaveStudents(IEnumerable<RawStudentInfo> students);

        [OperationContract]
        ExcelDocumentImportResult GetRecords(IEnumerable<RawStudentInfo> studentsRawInformation, string culture);

        [OperationContract]
        StudentInfo GetStudent(int id);

        [OperationContract]
        List<StudentInfo> GetStudents();

        [OperationContract]
        List<StudentCourse> GetStudentsCourses(int studentId);
        
        [OperationContract]
        void ApplyForCourse(int studentId, int courseId);

        [OperationContract]
        void LeaveCourse(int studentId, int courseId);

        [OperationContract]
        string GetStudentFullName(int studentId);

        [OperationContract]
        void UpdateStudent(int id, string firstName, string lastName, string age, string sex);

        [OperationContract]
        string GetStudentsSex(int id);

        [OperationContract]
        UiSettings GetUiColours(string id);

        [OperationContract]
        UiSettings GetUiColoursByName(string name);

        [OperationContract]
        void SetUiColours(string id, string backgroundColour, string headerColour);
   }
}
