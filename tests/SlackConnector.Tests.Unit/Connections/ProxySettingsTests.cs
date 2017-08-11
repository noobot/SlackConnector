using System;
using NUnit.Framework;
using SlackConnector.Connections;

namespace SlackConnector.Tests.Unit.Connections
{
    [TestFixture]
    public class ProxySettingsTests
    {
        [Test]
        public void should_set_properties_when_values_are_given()
        {
            // given
            const string url = "my-url";
            const string username = "usernameyy";
            const string password = "pwd123";

            // when
            var settings = new ProxySettings(url, username, password);

            // then
            Assert.That(settings.Url, Is.EqualTo(url));
            Assert.That(settings.Username, Is.EqualTo(username));
            Assert.That(settings.Password, Is.EqualTo(password));
        }

        [TestCase("")]
        [TestCase(null)]
        public void should_throw_exception_when_url_is_missing(string url)
        {
            // given
            const string username = "usernameyy";
            const string password = "pwd123";

            // when + then
            Assert.Throws<ArgumentNullException>(() => new ProxySettings(url, username, password), "url");
        }

        [TestCase("")]
        [TestCase(null)]
        public void should_throw_exception_when_username_is_missing(string username)
        {
            // given
            const string url = "my-url";
            const string password = "pwd123";

            // when + then
            Assert.Throws<ArgumentNullException>(() => new ProxySettings(url, username, password), "username");
        }

        [TestCase("")]
        [TestCase(null)]
        public void should_throw_exception_when_password_is_missing(string password)
        {
            // given
            const string url = "my-url";
            const string username = "usernameyy";

            // when + then
            Assert.Throws<ArgumentNullException>(() => new ProxySettings(url, username, password), "password");
        }
    }
}