using SlackConnector.Connections.Models;
using SlackConnector.Extensions;
using Xunit;
using Shouldly;

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
            user.Id.ShouldBe(slackUser.Id);
            user.Name.ShouldBe(slackUser.Name);
            user.Profile.Email.ShouldBe(slackUser.Email);
            user.TimeZoneOffset.ShouldBe(slackUser.TimeZoneOffset);
            user.IsBot.ShouldBe(slackUser.IsBot);
            user.Profile.FirstName.ShouldBe(slackUser.FirstName);
            user.Profile.LastName.ShouldBe(slackUser.LastName);
            user.Profile.Image.ShouldBe(slackUser.Image);
            user.Profile.Title.ShouldBe(slackUser.WhatIDo);
            user.Deleted.ShouldBe(slackUser.Deleted);
            slackUser.Online.HasValue.ShouldBeTrue();
            slackUser.Online.Value.ShouldBeTrue();
            slackUser.IsGuest.ShouldBeFalse();
            slackUser.StatusText.ShouldBe(user.Profile.StatusText);
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