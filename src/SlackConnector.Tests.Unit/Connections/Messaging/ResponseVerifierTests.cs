using System.Net;
using NUnit.Framework;
using RestSharp;
using Should;
using SlackConnector.Connections.Messaging;
using SlackConnector.Connections.Responses;
using SlackConnector.Exceptions;
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

        internal class given_invalid_http_response_then_should_throw_exception : SpecsFor<ResponseVerifier>
        {
            private IRestResponse restResponse;

            protected override void Given()
            {
                restResponse = new RestResponse
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
            }


            [Test]
            public void then_should_throw_expected_exception()
            {
                CommunicationException exception = null;

                try
                {
                    SUT.VerifyResponse<ExampleModel>(restResponse);
                }
                catch (CommunicationException ex)
                {
                    exception = ex;
                }

                string expectedMessage = $"Error occured while sending message '{restResponse.StatusCode}'";
                exception.ShouldNotBeNull();
                exception.Message.ShouldEqual(expectedMessage);
            }
        }

        internal class given_error_in_response_json_then_should_throw_exception : SpecsFor<ResponseVerifier>
        {
            private IRestResponse restResponse;

            protected override void Given()
            {
                restResponse = new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = @"{'ok': false, 'error': 'test error'}"
                };
            }


            [Test]
            public void then_should_throw_expected_exception()
            {
                CommunicationException exception = null;

                try
                {
                    SUT.VerifyResponse<ExampleModel>(restResponse);
                }
                catch (CommunicationException ex)
                {
                    exception = ex;
                }
                
                exception.ShouldNotBeNull();
                exception.Message.ShouldEqual("Error occured while posting message 'test error'");
            }
        }

        private class ExampleModel : StandardResponse
        {
            public string Value { get; set; }
        }
    }
}