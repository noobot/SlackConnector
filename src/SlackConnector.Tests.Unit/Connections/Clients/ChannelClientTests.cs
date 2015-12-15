using System.Linq;
using NUnit.Framework;
using RestSharp;
using Should;
using SlackConnector.Connections.Clients.Channel;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.Connections.Clients
{
    public static class ChannelClientTests
    {
        internal class given_valid_standard_setup : SpecsFor<ChannelClient>
        {
            private string _slackKey = "super-key";
            private string _user = "super-user";
            private RequestExecutorStub _requestExecutorStub;
            private JoinChannelResponse _executorResponse;
            private Channel Result { get; set; }

            protected override void InitializeClassUnderTest()
            {
                _requestExecutorStub = new RequestExecutorStub();
                SUT = new ChannelClient(_requestExecutorStub);
            }

            protected override void Given()
            {
                _executorResponse = new JoinChannelResponse
                {
                    Channel = new Channel()
                };
                _requestExecutorStub.Execute_Value = _executorResponse;
            }

            protected override void When()
            {
                Result = SUT.JoinDirectMessageChannel(_slackKey, _user).Result;
            }

            [Test]
            public void then_should_pass_expected_key()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("token"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_slackKey);
            }

            [Test]
            public void then_should_pass_expected_channel()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("user"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_user);
            }

            [Test]
            public void then_should_access_expected_path()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                request.Resource.ShouldEqual(ChannelClient.JOIN_DM_PATH);
            }

            [Test]
            public void then_should_have_2_params()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                request.Parameters.Count.ShouldEqual(2);
            }

            [Test]
            public void then_should_return_expected_channel()
            {
                Result.ShouldEqual(_executorResponse.Channel);
            }
        }
    }
}