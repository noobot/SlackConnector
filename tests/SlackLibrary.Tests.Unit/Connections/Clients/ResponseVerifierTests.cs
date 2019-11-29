using SlackLibrary.Connections.Clients;
using SlackLibrary.Connections.Responses;
using SlackLibrary.Exceptions;
using Xunit;
using Shouldly;

namespace SlackLibrary.Tests.Unit.Connections.Clients
{
    public class ResponseVerifierTests
    {
        [Fact]
        public void should_throw_exception_with_given_error_message_when_request_failed()
        {
            // given
            var response = new DefaultStandardResponse { Ok = false, Error = "I AM A ERROR-message" };
            var verifier = new ResponseVerifier();

            // when && then
            var exception = Assert.Throws<ResponseCommunicationException>(() => verifier.VerifyResponse(response));
            exception.Message.ShouldBe($"Error occured while posting message '{response.Error}'");
        }

        [Fact]
        public void should_not_throw_exception()
        {
            // given
            var response = new DefaultStandardResponse { Ok = true };
            var verifier = new ResponseVerifier();

            // when && then
            verifier.VerifyResponse(response);
        }
    }
}