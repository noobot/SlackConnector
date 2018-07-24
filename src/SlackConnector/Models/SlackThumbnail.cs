using System;

namespace SlackConnector.Models
{
    public class SlackThumbnail
    {
        public Uri Thumb64 { get; }
        public Uri Thumb80 { get; }
        public Uri Thumb360 { get; }
        public int Thumb360Width { get; }
        public int Thumb360Height { get; }
        public Uri Thumb160 { get; }
        public Uri Thumb360Gif { get; }

        public SlackThumbnail(
            Uri thumb64,
            Uri thumb80,
            Uri thumb360,
            int thumb360Width,
            int thumb360Height,
            Uri thumb160,
            Uri thumb360Gif)
        {
            Thumb64 = thumb64;
            Thumb80 = thumb80;
            Thumb360 = thumb360;
            Thumb360Width = thumb360Width;
            Thumb360Height = thumb360Height;
            Thumb160 = thumb160;
            Thumb360Gif = thumb360Gif;
        }
    }
}