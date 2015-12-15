using System.Linq;
using NUnit.Framework;
using RestSharp;
using Should;
using SlackConnector.Connections.Clients.Handshake;
using SlackConnector.Connections.Responses;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.Connections.Clients
{
    public static class HandshakeClientTests
    {
        internal class given_valid_response_when_handshaking_with_slack : SpecsFor<HandshakeClient>
        {
            private const string SLACK_KEY = "something_that_looks_like_a_key";
            private RequestExecutorStub _requestExecutorStub;
            private HandshakeResponse Result { get; set; }

            protected override void InitializeClassUnderTest()
            {
                _requestExecutorStub = new RequestExecutorStub();
                SUT = new HandshakeClient(_requestExecutorStub);
            }

            protected override void Given()
            {
                _requestExecutorStub.Execute_Value = new HandshakeResponse();
            }

            protected override void When()
            {
                Result = SUT.FirmShake(SLACK_KEY).Result;
            }

            [Test]
            public void then_should_pass_expected_key()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("token"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(SLACK_KEY);
            }

            [Test]
            public void then_should_access_expected_path()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                request.Resource.ShouldEqual(HandshakeClient.HANDSHAKE_PATH);
            }

            [Test]
            public void then_should_return_expected_model()
            {
                Result.ShouldEqual(_requestExecutorStub.Execute_Value);
            }
        }
    }
}