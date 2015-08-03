using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentTracker.Domain.Entities;

namespace StudentTracker.Domain.Configurations
{
    class UiColourConfiguration: EntityTypeConfiguration<UiColour>
    {
        public UiColourConfiguration()
        {
            this.HasKey(x => x.UserId);
            this.HasRequired(x => x.User).WithOptional(u => u.UiColour).WillCascadeOnDelete(true);
        }
    }
}
