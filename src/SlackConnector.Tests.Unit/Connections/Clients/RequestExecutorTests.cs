using Moq;
using NUnit.Framework;
using RestSharp;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Responses;
using SpecsFor;

namespace SlackConnector.Tests.Unit.Connections.Clients
{
    public static class RequestExecutorTests
    {
        internal class given_request_when_executing : SpecsFor<RestSharpRestSharpRequestExecutor>
        {
            private IRestRequest _request;
            private IRestResponse _response;
            private ExampleResponse _expectedResult;
            private ExampleResponse Result { get; set; }

            protected override void Given()
            {
                _request = new RestRequest();
                _response = new RestResponse();

                GetMockFor<IRestSharpFactory>()
                    .Setup(x => x.CreateClient(RestSharpRestSharpRequestExecutor.SLACK_URL))
                    .Returns(GetMockFor<IRestClient>().Object);

                GetMockFor<IRestClient>()
                    .Setup(x => x.ExecutePostTaskAsync(_request))
                    .ReturnsAsync(_response);

                _expectedResult = new ExampleResponse();
                GetMockFor<IResponseVerifier>()
                    .Setup(x => x.VerifyResponse<ExampleResponse>(_response))
                    .Returns(_expectedResult);
            }

            protected override void When()
            {
                Result = SUT.Execute<ExampleResponse>(_request).Result;
            }

            [Test]
            public void then_should_return_expected_result()
            {
                Result.ShouldEqual(_expectedResult);
            }
        }

        private class ExampleResponse : StandardResponse
        {
            
        }
    }
}