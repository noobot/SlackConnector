using System;
using System.Collections.Generic;
using System.Linq;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;

namespace SlackConnector.Extensions
{
    internal static class FileExtensions
    {
        public static IEnumerable<SlackFile> ToSlackFiles(this IEnumerable<File> file)
        {
            if (file == null)
            {
                return Enumerable.Empty<SlackFile>();
            }

            return file.Select(ToSlackFile);
        }

        private static SlackFile ToSlackFile(this File file)
        {
            if (file == null)
                return null;

            return new SlackFile(
                file.Id,
                file.Created,
                file.Timestamp,
                file.Name,
                file.Title,
                file.Mimetype,
                file.FileType,
                file.PrettyType,
                file.User,
                file.Editable,
                file.Size,
                file.Mode,
                file.IsExternal,
                file.ExternalType,
                file.IsPublic,
                file.PublicUrlShared,
                file.DisplayAsBot,
                file.Username,
                CreateUri(file.UrlPrivate),
                CreateUri(file.UrlPrivateDownload),
                file.ImageExifRotation,
                file.OriginalWidth,
                file.OriginalHeight,
                CreateUri(file.DeanimateGif),
                CreateUri(file.Permalink),
                CreateUri(file.PermalinkPublic),
                new SlackThumbnail(
                    CreateUri(file.Thumb64),
                    CreateUri(file.Thumb80),
                    CreateUri(file.Thumb360),
                    file.Thumb360Width,
                    file.Thumb360Height,
                    CreateUri(file.Thumb160),
                    CreateUri(file.Thumb360Gif)
                )
            );
        }

        private static Uri CreateUri(string url)
        {
            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri)
                ? uri
                : null;
        }
    }
}