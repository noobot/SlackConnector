using NUnit.Framework;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Handshaking.Models;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.Connections.Handshaking
{
    public static class HandshakerTests
    {
        internal class given_valid_response_when_handshaking_with_slack : SpecsFor<HandshakeClient>
        {
            private const string SLACK_KEY = "something_that_looks_like_a_key";
            private RestClientStub RestClient { get; set; }
            private SlackHandshake Result { get; set; }

            protected override void Given()
            {
                RestClient = new RestClientStub();

                GetMockFor<IRestSharpFactory>()
                    .Setup(x => x.CreateClient("https://slack.com"))
                    .Returns(RestClient);
            }

            protected override void When()
            {
                Result = SUT.FirmShake(SLACK_KEY).Result;
            }

            [Test]
            public void then_should_pass_expected_key()
            {
                
            }

            [Test]
            public void then_should_access_expected_path()
            {
                
            }

            [Test]
            public void then_should_return_expected_model()
            {
                
            }
        }
    }
}