using NUnit.Framework;
using SlackConnector.Connections.Models;
using SlackConnector.Extensions;

namespace SlackConnector.Tests.Unit.Extensions
{
    [TestFixture]
    public class UserExtensionsTests
    {
        [Test]
        public void should_create_slack_user_from_user()
        {
            var user = new User
            {
                Id = "Id",
                Name = "Name",
                TimeZoneOffset = 0L,
                IsBot = false,
                Deleted = false,
                Presence = "active",
                Profile = new Profile
                {
                    Email = "a@b.c",
                    FirstName = "First",
                    LastName = "Last",
                    Image = "http://image.com",
                    Title = "Developer"
                }
            };

            var slackUser = user.ToSlackUser();

            Assert.AreEqual(user.Id, slackUser.Id);
            Assert.AreEqual(user.Name, slackUser.Name);
            Assert.AreEqual(user.Profile.Email, slackUser.Email);
            Assert.AreEqual(user.TimeZoneOffset, slackUser.TimeZoneOffset);
            Assert.AreEqual(user.IsBot, slackUser.IsBot);
            Assert.AreEqual(user.Profile.FirstName, slackUser.FirstName);
            Assert.AreEqual(user.Profile.LastName, slackUser.LastName);
            Assert.AreEqual(user.Profile.Image, slackUser.Image);
            Assert.AreEqual(user.Profile.Title, slackUser.WhatIDo);
            Assert.AreEqual(user.Deleted, slackUser.Deleted);
            Assert.IsNotNull(slackUser.Online);
            Assert.IsTrue(slackUser.Online.Value);
            Assert.IsFalse(slackUser.IsGuest);
        }

        [Test]
        public void should_create_slack_user_from_incomplete_user()
        {
            var user = new User { Presence = "Away" };

            var slackUser = user.ToSlackUser();

            Assert.IsNotNull(slackUser.Online);
            Assert.IsFalse(slackUser.Online.Value);
        }

        [Test]
        public void should_create_guest_slack_user_from_guest_user() 
        {
            var user = new User 
            {
                IsGuest = true
            };

            var slackUser = user.ToSlackUser();

            Assert.IsTrue(slackUser.IsGuest);
        }
    }
}