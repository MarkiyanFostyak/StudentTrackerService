using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;
using StudentTracker.Domain.Entities;

namespace StudentTrackerService
{
    [DataContract]
    public class RawStudentInfo
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Sex { get; set; }

        [DataMember]
        public string Age { get; set; }

        public Student ToStudent()
        {
            return new Student()
            {
                FirstName = FirstName,
                LastName = LastName,
                Sex = Sex.Equals("Male", StringComparison.OrdinalIgnoreCase) ? StudentTracker.Domain.Entities.Sex.Male : StudentTracker.Domain.Entities.Sex.Female,
                Age = int.Parse(Age)
            };
        }

        public InvalidRecord ToInvalidRecord(string comment)
        {
            return new InvalidRecord
            {
                FirstName = FirstName,
                LastName = LastName,
                Age = Age,
                Sex = Sex,
                Comment = comment
            };
        }

        public override bool Equals(object obj)
        {
            RawStudentInfo info = obj as RawStudentInfo;
            if (info != null)
            {
                return this.FirstName == info.FirstName && this.LastName == info.LastName && this.Sex == info.Sex && this.Age == info.Age;
            }
            return false;
        }
    }
}