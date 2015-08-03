using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace StudentTrackerService
{
    [DataContract]
    public class StudentCourse
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string CourseName { get; set; }
        [DataMember]
        public int AssignedStudents { get; set; }
        [DataMember]
        public int MaxStudents { get; set; }
        [DataMember]
        public bool IsAppliedFor { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

        public override bool Equals(object obj)
        {
            StudentCourse course = obj as StudentCourse;
            if (course != null)
            {
                return this.Id.Equals(course.Id) &&
                    this.CourseName.Equals(course.CourseName) &&
                    this.AssignedStudents.Equals(course.AssignedStudents) &&
                    this.MaxStudents.Equals(course.MaxStudents) &&
                    this.IsAppliedFor.Equals(course.IsAppliedFor) &&
                    this.IsActive.Equals(course.IsActive);
            }
            return false;
        }
    }
}