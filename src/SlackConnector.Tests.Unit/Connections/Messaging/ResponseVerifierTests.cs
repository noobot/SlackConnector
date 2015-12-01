using System.Net;
using Microsoft.SqlServer.Server;
using NUnit.Framework;
using RestSharp;
using SlackConnector.Connections.Messaging;
using SlackConnector.Connections.Responses;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.Connections.Messaging
{
    public static class ResponseVerifierTests
    {
        internal class given_valid_response_then_should_return_expected_object : SpecsFor<ResponseVerifier>
        {
            private IRestResponse restResponse;
            private ExampleModel Result { get; set; }

            protected override void Given()
            {
                restResponse = new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = @"{'ok': true, 'value': 'test'}"
                };
            }

            protected override void When()
            {
                Result = SUT.VerifyResponse<ExampleModel>(restResponse);
            }

            [Test]
            public void then_should_return_expected_model()
            {
                var expected = new ExampleModel
                {
                    Ok = true,
                    Value = "test"
                };
                Result.ShouldLookLike(expected);
            }
        }

        private class ExampleModel : StandardResponse
        {
            public string Value { get; set; }
        }
    }
}