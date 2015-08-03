using System;
using NUnit;
using NUnit.Framework;
using Moq;
using StudentTracker.Domain;
using StudentTracker.Domain.Concrete;
using StudentTracker.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using StudentTrackerService;
using System.Data.Entity;
using StudentTracker.Domain.IdentityEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StudentTrackerServiceTests
{
    [TestFixture]
    public class SetUiSettingsTest
    {
        private StudentTrackerService.StudentTrackerService stService;
        UiColour colours;
        UiSettings expectedResult = new UiSettings { BackgroundColour = "#123321", HeaderColour = "#abccba" };
        ApplicationUser user5 = new ApplicationUser { Id = "5test", UserName = "nameTest", Email = "testem@test.com" };
        ApplicationUser user16 = new ApplicationUser { Id = "16test", UserName = "nameTestTwo", Email = "testeem2@test.com" };
        ApplicationUser user22 = new ApplicationUser { Id = "22test", UserName = "nameTestThree", Email = "testem3@test.com" };

        [SetUp]
        public void Initialize()
        {
            stService = new StudentTrackerService.StudentTrackerService();
            colours = new UiColour { BackgroundColour = "#123321", HeaderColour = "#abccba" };
            using (var ctx = new StudentTrackerDbContext())
            {
                ctx.Users.Add(user5);
                ctx.Users.Add(user16);
                ctx.Users.Add(user22);
                ctx.SaveChanges();
                ctx.Users.Find(user5.Id).UiColour = colours;
                ctx.SaveChanges();
            }
        }

        [TearDown]
        public void CleanUp()
        {
            using (var ctx = new StudentTrackerDbContext())
            {            
                foreach (var user in ctx.Users.Where(x=>x.Id==user5.Id || x.Id==user16.Id || x.Id==user22.Id))
                {
                    ctx.Users.Remove(user);
                }
                ctx.SaveChanges();
            }
        }

        [Test]
        public void SetUiColoursSuccess()
        {
            stService.SetUiColours(user5.Id, expectedResult.BackgroundColour, expectedResult.HeaderColour);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(expectedResult.BackgroundColour, ctx.Users.Find(user5.Id).UiColour.BackgroundColour);
                Assert.AreEqual(expectedResult.HeaderColour, ctx.Users.Find(user5.Id).UiColour.HeaderColour);
            }
        }

        [Test]
        public void SetNewUiColoursSuccess()
        {
            stService.SetUiColours(user16.Id, expectedResult.BackgroundColour, expectedResult.HeaderColour);
            using (var ctx = new StudentTrackerDbContext())
            {
                Assert.AreEqual(expectedResult.BackgroundColour, ctx.Users.Find(user16.Id).UiColour.BackgroundColour);
                Assert.AreEqual(expectedResult.HeaderColour, ctx.Users.Find(user16.Id).UiColour.HeaderColour);
            }
        }

        [Test]
        public void SetUiColoursNoId()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate { stService.SetUiColours("-1", "#111111", "#111111"); });
        }
    }
}
