using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTracker.Domain.IdentityEntities;

namespace StudentTracker.Domain.Entities
{
    public class UiColour
    {
        public string UserId { get; set; }
        public string HeaderColour { get; set; }
        public string BackgroundColour { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
