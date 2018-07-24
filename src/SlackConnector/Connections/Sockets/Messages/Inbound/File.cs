using Newtonsoft.Json;

namespace SlackConnector.Connections.Sockets.Messages.Inbound
{
    internal class File
    {
        public string Id { get; set; }
        public int Created { get; set; }
        public int Timestamp { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Mimetype { get; set; }
        public string FileType { get; set; }

        [JsonProperty("pretty_type")]
        public string PrettyType { get; set; }

        public string User { get; set; }
        public bool Editable { get; set; }
        public int Size { get; set; }
        public string Mode { get; set; }

        [JsonProperty("is_external")]
        public bool IsExternal { get; set; }

        [JsonProperty("external_type")]
        public string ExternalType { get; set; }

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("public_url_shared")]
        public bool PublicUrlShared { get; set; }

        [JsonProperty("display_as_bot")]
        public bool DisplayAsBot { get; set; }

        public string Username { get; set; }

        [JsonProperty("url_private")]
        public string UrlPrivate { get; set; }

        [JsonProperty("url_private_download")]
        public string UrlPrivateDownload { get; set; }

        [JsonProperty("thumb_64")]
        public string Thumb64 { get; set; }

        [JsonProperty("thumb_80")]
        public string Thumb80 { get; set; }

        [JsonProperty("thumb_360")]
        public string Thumb360 { get; set; }

        [JsonProperty("thumb_360_w")]
        public int Thumb360Width { get; set; }

        [JsonProperty("thumb_360_h")]
        public int Thumb360Height { get; set; }

        [JsonProperty("thumb_160")]
        public string Thumb160 { get; set; }

        [JsonProperty("thumb_360_gif")]
        public string Thumb360Gif { get; set; }

        [JsonProperty("image_exif_rotation")]
        public int ImageExifRotation { get; set; }

        [JsonProperty("original_w")]
        public int OriginalWidth { get; set; }

        [JsonProperty("original_h")]
        public int OriginalHeight { get; set; }

        [JsonProperty("deanimate_gif")]
        public string DeanimateGif { get; set; }

        public string Permalink { get; set; }

        [JsonProperty("permalink_public")]
        public string PermalinkPublic { get; set; }
    }
}