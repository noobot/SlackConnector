using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using Should;
using SlackConnector.Connections.Clients.Chat;
using SlackConnector.Connections.Responses;
using SlackConnector.Models;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.Connections.Clients
{
    public static class ChatClientTests
    {
        internal class given_valid_standard_setup_when_posting_message_without_attachments : SpecsFor<RestSharpChatClient>
        {
            private string _slackKey = "super-key";
            private string _channel = "super-channel";
            private string _text = "boom-jiggy-boom";
            private RestSharpRequestExecutorStub _restSharpRequestExecutorStub;

            protected override void InitializeClassUnderTest()
            {
                _restSharpRequestExecutorStub = new RestSharpRequestExecutorStub();
                SUT = new RestSharpChatClient(_restSharpRequestExecutorStub);
            }

            protected override void Given()
            {
                _restSharpRequestExecutorStub.Execute_Value = new StandardResponse();
            }

            protected override void When()
            {
                SUT.PostMessage(_slackKey, _channel, _text, null).Wait();
            }

            [Test]
            public void then_should_pass_expected_key()
            {
                IRestRequest request = _restSharpRequestExecutorStub.Execute_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("token"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_slackKey);
            }

            [Test]
            public void then_should_pass_expected_channel()
            {
                IRestRequest request = _restSharpRequestExecutorStub.Execute_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("channel"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_channel);
            }

            [Test]
            public void then_should_pass_expected_text()
            {
                IRestRequest request = _restSharpRequestExecutorStub.Execute_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("text"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_text);
            }

            [Test]
            public void then_should_pass_expected_user_param()
            {
                IRestRequest request = _restSharpRequestExecutorStub.Execute_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("as_user"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual("true");
            }

            [Test]
            public void then_should_access_expected_path()
            {
                IRestRequest request = _restSharpRequestExecutorStub.Execute_Request;
                request.Resource.ShouldEqual(RestSharpChatClient.SEND_MESSAGE_PATH);
            }

            [Test]
            public void then_should_have_4_params()
            {
                IRestRequest request = _restSharpRequestExecutorStub.Execute_Request;
                request.Parameters.Count.ShouldEqual(4);
            }
        }

        internal class given_valid_standard_setup_when_posting_message_with_attachments : SpecsFor<RestSharpChatClient>
        {
            private List<SlackAttachment> _attachments;
            private RestSharpRequestExecutorStub _restSharpRequestExecutorStub;

            protected override void InitializeClassUnderTest()
            {
                _restSharpRequestExecutorStub = new RestSharpRequestExecutorStub();
                SUT = new RestSharpChatClient(_restSharpRequestExecutorStub);
            }

            protected override void Given()
            {
                _attachments = new List<SlackAttachment>
                {
                    new SlackAttachment
                    {
                        Text = "Some Text",
                        Title = "Some Title"
                    }
                };
            }

            protected override void When()
            {
                SUT.PostMessage(null, null, null, _attachments).Wait();
            }
            
            [Test]
            public void then_should_pass_expected_text()
            {
                IRestRequest request = _restSharpRequestExecutorStub.Execute_Request;
                Parameter attachments = request.Parameters.FirstOrDefault(x => x.Name.Equals("attachments"));
                attachments.ShouldNotBeNull();
                attachments.Type.ShouldEqual(ParameterType.GetOrPost);

                string attachmentsJson = attachments.Value as string;
                attachmentsJson.ShouldEqual(JsonConvert.SerializeObject(_attachments));
            }

            [Test]
            public void then_should_have_5_params()
            {
                IRestRequest request = _restSharpRequestExecutorStub.Execute_Request;
                request.Parameters.Count.ShouldEqual(5);
            }
        }
    }
}