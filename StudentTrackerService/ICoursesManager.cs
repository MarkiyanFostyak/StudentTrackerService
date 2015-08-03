using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using StudentTracker.Domain.Entities;

namespace StudentTrackerService
{
    [ServiceContract]
    public interface ICoursesManager
    {
        [OperationContract]
        List<CourseInfo> GetCourses();

        [OperationContract]
        void DeleteCourse(string courseId);

        [OperationContract]
        bool AddCourse(Course entity);

        [OperationContract]
        bool UpdateCourse(string id, string name, string capacity);
    }
}
