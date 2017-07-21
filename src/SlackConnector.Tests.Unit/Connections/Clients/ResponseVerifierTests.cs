using NUnit.Framework;
using Should;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Responses;
using SlackConnector.Exceptions;

namespace SlackConnector.Tests.Unit.Connections.Clients
{
    [TestFixture]
    public class ResponseVerifierTests
    {
        [Test]
        public void should_throw_exception_with_given_error_message_when_request_failed()
        {
            // given
            var response = new StandardResponse { Ok = false, Error = "I AM A ERROR-message" };
            var verifier = new ResponseVerifier();

            // when && then
            var exception = Assert.Throws<CommunicationException>(() => verifier.VerifyResponse(response));
            exception.Message.ShouldEqual($"Error occured while posting message '{response.Error}'");
        }

        [Test]
        public void should_not_throw_exception()
        {
            // given
            var response = new StandardResponse { Ok = true };
            var verifier = new ResponseVerifier();

            // when && then
            Assert.DoesNotThrow(() => verifier.VerifyResponse(response));
        }
    }
}