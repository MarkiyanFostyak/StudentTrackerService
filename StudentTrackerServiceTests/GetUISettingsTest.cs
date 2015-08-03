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
    public class GetUISettingsTest
    {
        private StudentTrackerService.StudentTrackerService stService;
        UiColour colours;
        UiSettings expectedResult = new UiSettings { BackgroundColour = "#123321", HeaderColour = "#abccba" };
        UiSettings defaultResult = new UiSettings { BackgroundColour = "#ffffff", HeaderColour = "#101010" };

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
        public void GetSettingsSuccess()
        {
            var actualResult = stService.GetUiColours(user5.Id);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void GetSettingsNoId()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate { stService.GetUiColours("-1"); });
        }

        [Test]
        public void GetSettingsByNameSuccess()
        {
            var actualResult = stService.GetUiColoursByName(user5.UserName);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void GetSettingsNoName()
        {
            Assert.Throws(typeof(InvalidOperationException), delegate { stService.GetUiColoursByName("-1"); });
        }

        [Test]
        public void GetDefaultSettingsById()
        {
            var actualResult = stService.GetUiColours(user16.Id);
            Assert.AreEqual(defaultResult, actualResult);
        }

        [Test]
        public void GetDefaultSettingsByName()
        {
            var actualResult = stService.GetUiColoursByName(user22.UserName);
            Assert.AreEqual(defaultResult, actualResult);
        }
    }
}
