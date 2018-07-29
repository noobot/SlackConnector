using System.Linq;
using AutoFixture;
using Shouldly;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Extensions;
using Xunit;

namespace SlackConnector.Tests.Unit.Extensions
{
    public class FileExtensionsTests
    {
        [Fact]
        private void should_return_empty_enumeration()
        {
            // given

            // when
            var slackFiles = ((File[])null).ToSlackFiles();

            // then
            slackFiles.ShouldBeEmpty();
        }

        [Fact]
        private void should_return_null_file_if_entry_is_null()
        {
            // given
            var files = new File[] { null };

            // when
            var slackFiles = files.ToSlackFiles();

            // then
            slackFiles.ShouldHaveSingleItem();
            slackFiles.First().ShouldBeNull();
        }


        [Theory, AutoMoqData]
        private void should_return_slack_message(Fixture fixture)
        {
            // given
            var file = fixture.Build<File>()
                .With(f => f.UrlPrivate, $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}")
                .With(f => f.UrlPrivateDownload, $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}")
                .With(f => f.Permalink, $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}")
                .With(f => f.PermalinkPublic, $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}")
                .With(f => f.Thumb160, $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}")
                .With(f => f.Thumb360, $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}")
                .With(f => f.Thumb360Gif, $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}")
                .With(f => f.Thumb64, $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}")
                .With(f => f.Thumb80, $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}")
                .With(f => f.DeanimateGif, $"https://{fixture.Create<string>()}.com/{fixture.Create<string>()}")
                .Create();

            var files = new[] { file };

            // when
            var slackFile = files.ToSlackFiles().FirstOrDefault();

            // then
            slackFile.ShouldNotBeNull();
            slackFile.Id.ShouldBe(file.Id);
            slackFile.Created.ShouldBe(file.Created);
            slackFile.Timestamp.ShouldBe(file.Timestamp);
            slackFile.Name.ShouldBe(file.Name);
            slackFile.Title.ShouldBe(file.Title);
            slackFile.Mimetype.ShouldBe(file.Mimetype);
            slackFile.FileType.ShouldBe(file.FileType);
            slackFile.PrettyType.ShouldBe(file.PrettyType);
            slackFile.User.ShouldBe(file.User);
            slackFile.Editable.ShouldBe(file.Editable);
            slackFile.Size.ShouldBe(file.Size);
            slackFile.Mode.ShouldBe(file.Mode);
            slackFile.IsExternal.ShouldBe(file.IsExternal);
            slackFile.ExternalType.ShouldBe(file.ExternalType);
            slackFile.IsPublic.ShouldBe(file.IsPublic);
            slackFile.PublicUrlShared.ShouldBe(file.PublicUrlShared);
            slackFile.DisplayAsBot.ShouldBe(file.DisplayAsBot);
            slackFile.Username.ShouldBe(file.Username);
            slackFile.UrlPrivate.AbsoluteUri.ShouldBe(file.UrlPrivate);
            slackFile.UrlPrivateDownload.AbsoluteUri.ShouldBe(file.UrlPrivateDownload);
            slackFile.ImageExifRotation.ShouldBe(file.ImageExifRotation);
            slackFile.OriginalWidth.ShouldBe(file.OriginalWidth);
            slackFile.OriginalHeight.ShouldBe(file.OriginalHeight);
            slackFile.DeanimateGif.AbsoluteUri.ShouldBe(file.DeanimateGif);
            slackFile.Permalink.AbsoluteUri.ShouldBe(file.Permalink);
            slackFile.PermalinkPublic.AbsoluteUri.ShouldBe(file.PermalinkPublic);

            slackFile.Thumbnail.ShouldNotBeNull();
            slackFile.Thumbnail.Thumb64.AbsoluteUri.ShouldBe(file.Thumb64);
            slackFile.Thumbnail.Thumb80.AbsoluteUri.ShouldBe(file.Thumb80);
            slackFile.Thumbnail.Thumb360.AbsoluteUri.ShouldBe(file.Thumb360);
            slackFile.Thumbnail.Thumb360Width.ShouldBe(file.Thumb360Width);
            slackFile.Thumbnail.Thumb360Height.ShouldBe(file.Thumb360Height);
            slackFile.Thumbnail.Thumb160.AbsoluteUri.ShouldBe(file.Thumb160);
            slackFile.Thumbnail.Thumb360Gif.AbsoluteUri.ShouldBe(file.Thumb360Gif);
        }
    }
}