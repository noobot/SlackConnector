using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using NUnit.Framework;
using RestSharp;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Messaging;
using SlackConnector.Connections.Models;
using SlackConnector.Exceptions;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.Connections.Messaging
{
    public static class ChannelMessengerTests
    {
        internal class given_valid_standard_setup : SpecsFor<ChannelMessenger>
        {
            private string _slackKey = "super-key";
            private string _user = "super-user";
            private RestClientStub RestStub { get; set; }
            private Channel Result { get; set; }

            protected override void Given()
            {
                RestStub = new RestClientStub
                {
                    ExecutePostTaskAsync_StatusCode = HttpStatusCode.OK,
                    ExecutePostTaskAsync_Content = @"{
                                                        'ok':true, 
                                                        'channel': {
                                                            'id': 'my-id',
                                                            'name': 'my-name',
                                                        }
                                                     }"
                };

                GetMockFor<IRestSharpFactory>()
                    .Setup(x => x.CreateClient("https://slack.com"))
                    .Returns(RestStub);
            }

            protected override void When()
            {
                Result = SUT.JoinDirectMessageChannel(_slackKey, _user).Result;
            }

            [Test]
            public void then_should_pass_expected_key()
            {
                IRestRequest request = RestStub.ExecutePostTaskAsync_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("token"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_slackKey);
            }

            [Test]
            public void then_should_pass_expected_channel()
            {
                IRestRequest request = RestStub.ExecutePostTaskAsync_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("user"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_user);
            }

            [Test]
            public void then_should_access_expected_path()
            {
                IRestRequest request = RestStub.ExecutePostTaskAsync_Request;
                request.Resource.ShouldEqual(ChannelMessenger.JOIN_DM_PATH);
            }

            [Test]
            public void then_should_have_2_params()
            {
                IRestRequest request = RestStub.ExecutePostTaskAsync_Request;
                request.Parameters.Count.ShouldEqual(2);
            }

            [Test]
            public void should_return_expected_channel()
            {
                var expected = new Channel
                {
                    Id = "my-id",
                    Name = "my-name"
                };

                Result.ShouldLookLike(expected);
            }
        }

        internal class given_error_occured_in_communications : SpecsFor<ChannelMessenger>
        {
            private RestClientStub RestStub { get; set; }

            protected override void Given()
            {
                RestStub = new RestClientStub
                {
                    ExecutePostTaskAsync_StatusCode = HttpStatusCode.BadGateway,
                    ExecutePostTaskAsync_Content = "{'ok':true}"
                };

                GetMockFor<IRestSharpFactory>()
                    .Setup(x => x.CreateClient("https://slack.com"))
                    .Returns(RestStub);
            }

            [Test]
            public void then_should_throw_exception()
            {
                CommunicationException exception = null;

                try
                {
                    SUT.JoinDirectMessageChannel("", "").Wait();
                }
                catch (AggregateException ex)
                {
                    exception = ex.InnerExceptions[0] as CommunicationException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo($"Error occured while joining channel '{RestStub.ExecutePostTaskAsync_StatusCode}'"));
            }
        }

        internal class given_error_occured_in_response_json : SpecsFor<ChannelMessenger>
        {
            private RestClientStub RestStub { get; set; }

            protected override void Given()
            {
                RestStub = new RestClientStub
                {
                    ExecutePostTaskAsync_StatusCode = HttpStatusCode.OK,
                    ExecutePostTaskAsync_Content = "{'ok':false,'error':'blooming error'}"
                };

                GetMockFor<IRestSharpFactory>()
                    .Setup(x => x.CreateClient("https://slack.com"))
                    .Returns(RestStub);
            }

            [Test]
            public void then_should_throw_exception()
            {
                CommunicationException exception = null;

                try
                {
                    SUT.JoinDirectMessageChannel("", "").Wait();
                }
                catch (AggregateException ex)
                {
                    exception = ex.InnerExceptions[0] as CommunicationException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo("Error occured while joining channel 'blooming error'"));
            }
        }
    }
}