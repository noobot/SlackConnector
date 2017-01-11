using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RestSharp;
using Should;
using SlackConnector.Connections.Clients.File;
using SlackConnector.Tests.Unit.Stubs;
using SpecsFor;

namespace SlackConnector.Tests.Unit.Connections.Clients
{
    public static class FileClientTests
    {
        internal class given_valid_standard_setup_when_posting_file : SpecsFor<FileClient>
        {
            private readonly string _slackKey = "super-key";
            private readonly string _channelId = "channel-id";
            private readonly string _filePath = Path.GetTempFileName();
            private RequestExecutorStub _requestExecutorStub;

            protected override void InitializeClassUnderTest()
            {
                _requestExecutorStub = new RequestExecutorStub();
                SUT = new FileClient(_requestExecutorStub);
            }

            protected override void Given()
            {
                File.WriteAllText(_filePath, "test-data");
            }

            protected override void When()
            {
                SUT.PostFile(_slackKey, _channelId, _filePath).Wait();
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
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("channels"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_channelId);
            }

            [Test]
            public void then_should_access_expected_path()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                request.Resource.ShouldEqual(FileClient.FILE_UPLOAD_PATH);
            }

            [Test]
            public void then_should_have_3_params()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                request.Parameters.Count.ShouldEqual(3);
            }

            [Test]
            public void then_should_pass_expected_filename()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("filename"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(Path.GetFileName(_filePath));
            }

            [Test]
            public void then_should_pass_expected_file()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                FileParameter file = request.Files.FirstOrDefault(x => x.Name.Equals("file"));
                file.ShouldNotBeNull();
                file.ContentLength.ShouldBeGreaterThan(0);
                file.FileName.ShouldEqual(Path.GetFileName(_filePath));
            }
        }

        internal class given_valid_standard_setup_when_posting_stream : SpecsFor<FileClient>
        {
            private readonly string _slackKey = "super-key";
            private readonly string _channelId = "channel-id";
            private readonly string _fileName = "file-name.txt";
            private MemoryStream _inputStream;
            private RequestExecutorStub _requestExecutorStub;

            protected override void InitializeClassUnderTest()
            {
                _requestExecutorStub = new RequestExecutorStub();
                SUT = new FileClient(_requestExecutorStub);
            }

            protected override void Given()
            {
                _inputStream = new MemoryStream();
                using (var writer = new StreamWriter(_inputStream, Encoding.UTF8))
                {
                    writer.WriteLine("Some test value - yay :-)");
                }
            }

            protected override void When()
            {
                SUT.PostFile(_slackKey, _channelId, _inputStream, _fileName).Wait();
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
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("channels"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(_channelId);
            }

            [Test]
            public void then_should_access_expected_path()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                request.Resource.ShouldEqual(FileClient.FILE_UPLOAD_PATH);
            }

            [Test]
            public void then_should_have_3_params()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                request.Parameters.Count.ShouldEqual(3);
            }

            [Test]
            public void then_should_pass_expected_filename()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                Parameter keyParam = request.Parameters.FirstOrDefault(x => x.Name.Equals("filename"));
                keyParam.ShouldNotBeNull();
                keyParam.Type.ShouldEqual(ParameterType.GetOrPost);
                keyParam.Value.ShouldEqual(Path.GetFileName(_fileName));
            }

            [Test]
            public void then_should_pass_expected_file()
            {
                IRestRequest request = _requestExecutorStub.Execute_Request;
                FileParameter file = request.Files.FirstOrDefault(x => x.Name.Equals("file"));
                file.ShouldNotBeNull();
                file.ContentLength.ShouldBeGreaterThan(0);
                file.FileName.ShouldEqual(Path.GetFileName(_fileName));
            }
        }
    }
}