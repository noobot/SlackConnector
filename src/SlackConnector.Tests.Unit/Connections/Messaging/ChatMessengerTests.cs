using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using Should;
using SlackConnector.Connections;
using SlackConnector.Connections.Messaging;
using SlackConnector.Exceptions;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;
using SpecsFor.ShouldExtensions;

namespace SlackConnector.Tests.Unit.Connections.Messaging
{
    public static class ChatMessengerTests
    {
        internal class given_valid_standard_setup : SpecsFor<ChatMessenger>
        {
            private string _slackKey = "super-key";
            private string _channel = "super-channel";
            private string _text = "boom-jiggy-boom";
            private RestClientStub RestStub { get; set; }

            protected override void Given()
            {
                RestStub = new RestClientStub
                {
                    ExecutePostTaskAsync_StatusCode = HttpStatusCode.OK,
                    ExecutePostTaskAsync_Content = "{'ok':true}"
                };

                GetMockFor<IRestSharpFactory>()
                    .Setup(x => x.CreateClient("https://slack.com"))
                    .Returns(RestStub);
            }

            protected override void When()
            {
                SUT.PostMessage(_slackKey, _channel, _text, null).Wait();
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
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("channel"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_channel);
            }

            [Test]
            public void then_should_pass_expected_text()
            {
                IRestRequest request = RestStub.ExecutePostTaskAsync_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("text"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_text);
            }

            [Test]
            public void then_should_pass_expected_user_param()
            {
                IRestRequest request = RestStub.ExecutePostTaskAsync_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("as_user"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual("true");
            }

            [Test]
            public void then_should_access_expected_path()
            {
                IRestRequest request = RestStub.ExecutePostTaskAsync_Request;
                request.Resource.ShouldEqual(ChatMessenger.SEND_MESSAGE_PATH);
            }

            [Test]
            public void then_should_have_4_params()
            {
                IRestRequest request = RestStub.ExecutePostTaskAsync_Request;
                request.Parameters.Count.ShouldEqual(4);
            }
        }

        internal class given_error_occured_in_communications : SpecsFor<ChatMessenger>
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
                    SUT.PostMessage("", "", "", null).Wait();
                }
                catch (AggregateException ex)
                {
                    exception = ex.InnerExceptions[0] as CommunicationException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo($"Error occured while posting message '{RestStub.ExecutePostTaskAsync_StatusCode}'"));
            }
        }

        internal class given_error_occured_in_response_json : SpecsFor<ChatMessenger>
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
                    SUT.PostMessage("", "", "", null).Wait();
                }
                catch (AggregateException ex)
                {
                    exception = ex.InnerExceptions[0] as CommunicationException;
                }

                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.Message, Is.EqualTo($"Error occured while posting message 'blooming error'"));
            }
        }
    }
}