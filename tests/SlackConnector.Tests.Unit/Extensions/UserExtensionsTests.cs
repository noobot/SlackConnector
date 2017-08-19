using SlackConnector.Connections.Models;
using SlackConnector.Extensions;
using Xunit;
using Should;

namespace SlackConnector.Tests.Unit.Extensions
{
    public class UserExtensionsTests
    {
        [Fact]
        public void should_create_slack_user_from_user()
        {
            // given
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
                    Title = "Developer", 
                    StatusText = "Vacationing"
                }
            };

            // when
            var slackUser = user.ToSlackUser();

            // then
            user.Id.ShouldEqual(slackUser.Id);
            user.Name.ShouldEqual(slackUser.Name);
            user.Profile.Email.ShouldEqual(slackUser.Email);
            user.TimeZoneOffset.ShouldEqual(slackUser.TimeZoneOffset);
            user.IsBot.ShouldEqual(slackUser.IsBot);
            user.Profile.FirstName.ShouldEqual(slackUser.FirstName);
            user.Profile.LastName.ShouldEqual(slackUser.LastName);
            user.Profile.Image.ShouldEqual(slackUser.Image);
            user.Profile.Title.ShouldEqual(slackUser.WhatIDo);
            user.Deleted.ShouldEqual(slackUser.Deleted);
            slackUser.Online.HasValue.ShouldBeTrue();
            slackUser.Online.Value.ShouldBeTrue();
            slackUser.IsGuest.ShouldBeFalse();
            slackUser.StatusText.ShouldEqual(user.Profile.StatusText);
        }

        [Fact]
        public void should_create_slack_user_from_incomplete_user()
        {
            // given
            var user = new User { Presence = "Away" };

            // when
            var slackUser = user.ToSlackUser();

            // then
            slackUser.Online.HasValue.ShouldBeTrue();
            slackUser.Online.Value.ShouldBeFalse();
        }

        [Fact]
        public void should_create_guest_slack_user_from_guest_user() 
        {
            // given
            var user = new User
            {
                IsGuest = true
            };

            // when
            var slackUser = user.ToSlackUser();

            // then
            slackUser.IsGuest.ShouldBeTrue();
        }
    }
}