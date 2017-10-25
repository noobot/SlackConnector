using SlackConnector.Connections.Models;
using SlackConnector.Models;

namespace SlackConnector.Extensions
{
    internal static class UserExtensions
    {
        public static SlackUser ToSlackUser(this User user)
        {
            var slackUser = new SlackUser
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Profile?.Email,
                TimeZoneOffset = user.TimeZoneOffset,
                IsBot = user.IsBot,
                FirstName = user.Profile?.FirstName,
                LastName = user.Profile?.LastName,
                Image = user.Profile?.Image,
                WhatIDo = user.Profile?.Title,
                Deleted = user.Deleted,
                IsGuest = user.IsGuest,
                StatusText = user.Profile?.StatusText,
                IsAdmin = user.IsAdmin
            };

            if (!string.IsNullOrWhiteSpace(user.Presence))
            {
                slackUser.Online = user.Presence == "active";
            }

            return slackUser;
        }
    }
}