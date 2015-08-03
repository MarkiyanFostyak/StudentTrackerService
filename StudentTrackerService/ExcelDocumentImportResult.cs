using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace StudentTrackerService
{
    [DataContract]
    public class ExcelDocumentImportResult
    {
        [DataMember]
        public List<RawStudentInfo> ValidRecords { get; set; }

        [DataMember]
        public List<InvalidRecord> InvalidRecords { get; set; }

        public ExcelDocumentImportResult()
        {
            this.ValidRecords = new List<RawStudentInfo>();
            this.InvalidRecords = new List<InvalidRecord>();
        }

        public override bool Equals(object obj)
        {
            ExcelDocumentImportResult item = obj as ExcelDocumentImportResult;
            if (item != null)
            {
                return this.ValidRecords.SequenceEqual(item.ValidRecords) && this.InvalidRecords.SequenceEqual(item.InvalidRecords);
            }

            return false;
        }
    }
}