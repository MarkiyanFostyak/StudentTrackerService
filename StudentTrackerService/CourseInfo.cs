using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace StudentTrackerService
{
    [DataContract]
    public class CourseInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string CourseName { get; set; }

        [DataMember]
        public int CurrentlyAssigned { get; set; }

        [DataMember]
        public int MaxAssigned { get; set; }

        public override bool Equals(object obj)
        {
            CourseInfo course = obj as CourseInfo;
            if (course != null)
            {
                return this.Id == course.Id;
            }
            return false;
        }
    }
}