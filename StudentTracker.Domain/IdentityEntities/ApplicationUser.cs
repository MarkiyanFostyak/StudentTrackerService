using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Web;
using System;
using StudentTracker.Domain.Concrete;
using StudentTracker.Domain.Entities;
using StudentTracker.Domain.IdentityEntities;

namespace StudentTracker.Domain.IdentityEntities
{
    // You can add User data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual UiColour UiColour { get; set; }
    }
}
