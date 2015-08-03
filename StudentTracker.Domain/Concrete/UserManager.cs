using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentTracker.Domain.Concrete;
using StudentTracker.Domain.Entities;
using StudentTracker.Domain.IdentityEntities;

namespace StudentTracker.Domain.Concrete
{
    public class UserManager : UserManager<ApplicationUser>
    {
        public UserManager()
            : base(new UserStore<ApplicationUser>(new StudentTrackerDbContext()))
        {
        }
    }
}
