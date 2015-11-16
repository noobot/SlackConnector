using System;
using System.Linq;
using System.Net;
using NUnit.Framework;
using RestSharp;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Handshaking;
using SlackConnector.Connections.Models;
using SlackConnector.Connections.Responses;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.Connections.Handshaking
{
    public static class HandshakeClientTests
    {
        internal class given_valid_response_when_handshaking_with_slack : SpecsFor<HandshakeClient>
        {
            private const string SLACK_KEY = "something_that_looks_like_a_key";
            private RestClientStub RestStub { get; set; }
            private HandshakeResponse Result { get; set; }

            protected override void Given()
            {
                RestStub = new RestClientStub
                {
                    ExecutePostTaskAsync_Content = Resources.ResourceManager.GetHandShakeResponseJson(),
                    ExecutePostTaskAsync_StatusCode = HttpStatusCode.OK
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
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
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

        internal class given_http_error_when_handshaking : SpecsFor<HandshakeClient>
        {
            protected override void Given()
            {
                var stub = new RestClientStub
                {
                    ExecutePostTaskAsync_Content = string.Empty,
                    ExecutePostTaskAsync_StatusCode = HttpStatusCode.BadRequest
                };

                GetMockFor<IRestSharpFactory>()
                    .Setup(x => x.CreateClient("https://slack.com"))
                    .Returns(stub);
            }

            [Test]
            public void then_should_throw_exception()
            {
                bool exceptionDetected = false;

                try
                {
                    var something = SUT.FirmShake("something").Result;
                }
                catch (AggregateException ex)
                {
                    exceptionDetected = ex.InnerExceptions[0] is WebException;
                }

                Assert.That(exceptionDetected, Is.True);
            }
        }

        //TODO: When error is returned in JSON, display error
        //TODO: Maybe find all error types and turn into enum?
    }
}