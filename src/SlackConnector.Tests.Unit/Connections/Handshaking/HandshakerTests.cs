using System.Linq;
using NUnit.Framework;
using RestSharp;
using Should;
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
            private RestClientStub RestStub { get; set; }
            private SlackHandshake Result { get; set; }

            protected override void Given()
            {
                RestStub = new RestClientStub
                {
                    ExecutePostTaskAsync_Content = Resources.ResourceManager.GetHandShakeResponseJson()
                };

                GetMockFor<IRestSharpFactory>()
                    .Setup(x => x.CreateClient("https://slack.com"))
                    .Returns(RestStub);
            }

            protected override void When()
            {
                Result = SUT.FirmShake(SLACK_KEY).Result;
            }

            [Test]
            public void then_should_pass_expected_key()
            {
                IRestRequest request = RestStub.ExecutePostTaskAsync_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("token"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.HttpHeader);
                keyParam.Value.ShouldEqual(SLACK_KEY);
            }

            [Test]
            public void then_should_access_expected_path()
            {
                IRestRequest request = RestStub.ExecutePostTaskAsync_Request;
                request.Resource.ShouldEqual(HandshakeClient.HANDSHAKE_PATH);
            }

            [Test]
            public void then_should_return_expected_model()
            {
                
            }
        }
    }
}