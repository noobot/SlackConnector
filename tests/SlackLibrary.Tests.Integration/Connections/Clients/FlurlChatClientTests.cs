using Shouldly;
using SlackLibrary.Connections.Clients;
using SlackLibrary.Connections.Clients.Chat;
using SlackLibrary.Models.Blocks;
using SlackLibrary.Models.Blocks.Elements;
using SlackLibrary.Models.Blocks.Objects;
using SlackLibrary.Tests.Integration.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SlackLibrary.Tests.Integration.Connections.Clients
{
	public class FlurlChatClientTests
	{
		private IList<BlockBase> CreateBlocks()
		{
			var list = new List<BlockBase>();

			list.AddRange(new BlockBase[]{
				new SectionBlock() { Text = new TextObject("*section block with text*") },
				new SectionBlock() {
					Text = new TextObject("*section block with image*"),
					Accessory = new ImageElement("https://picsum.photos/200/300", "Picsum alt text")
				},
				new ImageBlock("https://picsum.photos/200/300", "Picsum alt text").WithTitle("Main title"),
				new DividerBlock(),
				new ContextBlock().AddMarkdownText("_MD Text_").AddImage("https://picsum.photos/200/300", "Picsum alt text"),
				new ActionsBlock().AddButton("action_button_action", "Action block 1")
				});

			return list;
		}

		[Fact]
		public async Task should_send_message_blocks_with_flurl()
		{
			// given
			var config = new ConfigReader().GetConfig();
			var client = new FlurlChatClient(new ResponseVerifier());
			var blocks = this.CreateBlocks();

			// when
			var response = await client.PostMessage(config.Slack.ApiToken,
				config.Slack.TestChannel, null, blocks: blocks);

			// then
			response.ShouldNotBeNull();
			response.Ok.ShouldBeTrue();
		}
	}
}
