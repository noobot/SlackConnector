using System.Linq;
using NUnit.Framework;
using RestSharp;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Messaging;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.Connections.Messaging
{
    public static class ChannelMessengerTests
    {
        internal class given_valid_standard_setup : SpecsFor<ChannelMessenger>
        {
            private string _slackKey = "super-key";
            private string _user = "super-user";
            private RestClientStub _restStub;
            private JoinChannelResponse _verifierResponse;
            private Channel Result { get; set; }

            protected override void Given()
            {
                _restStub = new RestClientStub
                {
                    ExecutePostTaskAsync_Response = new RestResponse()
                };

                GetMockFor<IRestSharpFactory>()
                    .Setup(x => x.CreateClient("https://slack.com"))
                    .Returns(_restStub);

                _verifierResponse = new JoinChannelResponse { Channel = new Channel() };
                GetMockFor<IResponseVerifier>()
                    .Setup(x => x.VerifyResponse<JoinChannelResponse>(_restStub.ExecutePostTaskAsync_Response))
                    .Returns(_verifierResponse);
            }

            protected override void When()
            {
                Result = SUT.JoinDirectMessageChannel(_slackKey, _user).Result;
            }

            [Test]
            public void then_should_pass_expected_key()
            {
                IRestRequest request = _restStub.ExecutePostTaskAsync_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("token"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_slackKey);
            }

            [Test]
            public void then_should_pass_expected_channel()
            {
                IRestRequest request = _restStub.ExecutePostTaskAsync_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("user"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_user);
            }

            [Test]
            public void then_should_access_expected_path()
            {
                IRestRequest request = _restStub.ExecutePostTaskAsync_Request;
                request.Resource.ShouldEqual(ChannelMessenger.JOIN_DM_PATH);
            }

            [Test]
            public void then_should_have_2_params()
            {
                IRestRequest request = _restStub.ExecutePostTaskAsync_Request;
                request.Parameters.Count.ShouldEqual(2);
            }

            [Test]
            public void then_should_return_expected_channel()
            {
                Result.ShouldEqual(_verifierResponse.Channel);
            }
        }
    }
}