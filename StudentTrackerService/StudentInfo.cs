using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace StudentTrackerService
{
    [DataContract]
    public class StudentInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Sex { get; set; }

        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public List<string> Courses { get; set; }

        public override bool Equals(object obj)
        {
            StudentInfo info = obj as StudentInfo;
            if (info != null)
            {
                return this.Id == info.Id && this.FirstName == info.FirstName && this.LastName == info.LastName && this.Sex == info.Sex && this.Age == info.Age;
            }
            return false;
        }
    }
}