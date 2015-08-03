using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace StudentTrackerService
{
    [DataContract]
    public class InvalidRecord: RawStudentInfo
    {
        [DataMember]
        public string Comment { get; set; }
        public override bool Equals(object obj)
        {
            InvalidRecord record = obj as InvalidRecord;
            if (record != null)
            {
                return base.FirstName.Equals(record.FirstName) && base.LastName.Equals(record.LastName) && base.Age.Equals(record.Age) && base.Sex.Equals(record.Sex) && this.Comment.Equals(record.Comment);
            }
            return false;
        }
    }
}