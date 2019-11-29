using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SixLabors.ImageSharp;

namespace SlackLibrary.Tests.Integration
{
	public class FileDownloadTests : IntegrationTest
	{
		[Fact]
		public async Task should_download_file()
		{
			// given
			var uri = new Uri("https://files.slack.com/files-pri/T3NFBGBAS-FBUSTA0P4/fuuuu.gif");

			// when
			var download = await SlackConnection.DownloadFile(uri);

			// then
			using (download)
			{
				download.ShouldNotBeNull();
				download.Length.ShouldBeGreaterThan(0);

				var image = Image.Load(download);
				image.Width.ShouldBe(43);
				image.Height.ShouldBe(29);
			}
		}
	}
}
