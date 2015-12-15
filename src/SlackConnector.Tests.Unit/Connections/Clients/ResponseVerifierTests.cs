using System.Net;
using NUnit.Framework;
using RestSharp;
using Should;
using SlackConnector.Connections.Clients;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using SlackConnector.Exceptions;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.Connections.Clients
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

        internal class given_join_channel_response : SpecsFor<ResponseVerifier>
        {
            private IRestResponse _restResponse;
            private JoinChannelResponse Result { get; set; }

            protected override void Given()
            {
                _restResponse = new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = @"{
                                    'ok':true, 
                                    'channel': {
                                        'id': 'my-id',
                                        'name': 'my-name',
                                    }
                                }"
                };
            }

            protected override void When()
            {
                Result = SUT.VerifyResponse<JoinChannelResponse>(_restResponse);
            }

            [Test]
            public void should_return_expected_channel_response()
            {
                var expected = new JoinChannelResponse
                {
                    Ok = true,
                    Error = null,
                    Channel = new Channel
                    {
                        Id = "my-id",
                        Name = "my-name"
                    }
                };

                Result.ShouldLookLike(expected);
            }
        }

        internal class given_handshake_response : SpecsFor<ResponseVerifier>
        {
            private IRestResponse _restResponse;
            private HandshakeResponse Result { get; set; }

            protected override void Given()
            {
                _restResponse = new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = Resources.ResourceManager.GetHandShakeResponseJson()
                };
            }

            protected override void When()
            {
                Result = SUT.VerifyResponse<HandshakeResponse>(_restResponse);
            }

            [Test]
            public void should_return_expected_channel_response()
            {
                var expected = new HandshakeResponse
                {
                    Ok = true,
                    Self = new Detail
                    {
                        Id = "self-id",
                        Name = "self-name"
                    },
                    Team = new Detail
                    {
                        Id = "team-id",
                        Name = "team-name"
                    },
                    Channels = new[]
                       {
                        new Channel
                        {
                            Id = "channel-id",
                            Name = "channel-name",
                            IsChannel = true,
                            IsArchived = true,
                            IsMember = true
                        }
                    },
                    Groups = new[]
                       {
                        new Group
                        {
                            Id = "group-id",
                            Name = "group-name",
                            IsGroup = true,
                            IsArchived = true,
                            IsOpen = true,
                            Members = new []
                            {
                                "member1"
                            }
                        }
                    },
                    Ims = new[]
                       {
                        new Im
                        {
                            Id = "im-id",
                            User = "im-user",
                            IsIm = true,
                            IsOpen = true
                        }
                    },
                    Users = new[]
                       {
                        new User
                        {
                            Id = "user-id",
                            Name = "user-name",
                            Deleted = true,
                            Profile = new Profile
                            {
                                FirstName = "first-name",
                                LastName = "last-name",
                                RealName = "real-name",
                                RealNameNormalised = "real-name-normalized",
                                Email = "email"
                            },
                            IsAdmin = true,
                            IsBot = true
                        }
                    },
                    WebSocketUrl = @"wss://ms331.slack-msgs.com/websocket/999"
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