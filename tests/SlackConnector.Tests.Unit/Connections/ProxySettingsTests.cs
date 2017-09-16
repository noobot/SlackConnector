using System;
using SlackConnector.Connections;
using Xunit;
using Shouldly;

namespace SlackConnector.Tests.Unit.Connections
{
    public class ProxySettingsTests
    {
        [Fact]
        public void should_set_properties_when_values_are_given()
        {
            // given
            const string url = "my-url";
            const string username = "usernameyy";
            const string password = "pwd123";

            // when
            var settings = new ProxySettings(url, username, password);

            // then
            settings.Url.ShouldBe(url);
            settings.Username.ShouldBe(username);
            settings.Password.ShouldBe(password);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void should_throw_exception_when_url_is_missing(string url)
        {
            // given
            const string username = "usernameyy";
            const string password = "pwd123";

            // when + then
            Assert.Throws<ArgumentNullException>("url", () => new ProxySettings(url, username, password));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void should_throw_exception_when_username_is_missing(string username)
        {
            // given
            const string url = "my-url";
            const string password = "pwd123";

            // when + then
            Assert.Throws<ArgumentNullException>("username", () => new ProxySettings(url, username, password));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void should_throw_exception_when_password_is_missing(string password)
        {
            // given
            const string url = "my-url";
            const string username = "usernameyy";

            // when + then
            Assert.Throws<ArgumentNullException>("password", () => new ProxySettings(url, username, password));
        }
    }
}