using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace StudentTrackerService
{
    [DataContract]
    public class UiSettings
    {
        [DataMember]
        public string BackgroundColour { get; set; }

        [DataMember]
        public string HeaderColour { get; set; }

        public override bool Equals(object obj)
        {
            UiSettings settings = obj as UiSettings;
            if (settings != null)
            {
                return this.BackgroundColour.Equals(settings.BackgroundColour) && this.HeaderColour.Equals(settings.HeaderColour);
            }
            return false;
        }
    }
}