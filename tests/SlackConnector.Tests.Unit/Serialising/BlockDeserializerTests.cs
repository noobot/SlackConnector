using SlackConnector.Models.Blocks;
using SlackConnector.Models.Blocks.Elements;
using SlackConnector.Serialising;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SlackConnector.Tests.Unit.Serialising
{
	public class BlockDeserializerTests
	{
		[Fact]
		public void WhenDeserializingMessageThenItWorks()
		{
			var deserializer = new BlockDeserializer();
			#region bigjson
			var json = @"[
	{
		""type"": ""section"",
		""text"": {
			""type"": ""mrkdwn"",
			""text"": ""Hi Laura, would you have some times to answer this question?""
		}
	},
	{
		""type"": ""divider""
	},
	{
		""type"": ""context"",
		""elements"": [
			{
				""type"": ""image"",
				""image_url"": ""https://images.pexels.com/photos/220453/pexels-photo-220453.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500"",
				""alt_text"": ""First name Last name | Job @Company""
			},
			{
				""type"": ""mrkdwn"",
				""text"": ""Alexandre Bousquet | CTO @ Comet""
			}
		]
	},
	{
		""type"": ""section"",
		""text"": {
			""type"": ""mrkdwn"",
			""text"": ""*Hi, do you have a good A/B testing tool to recommend to push features gradually to users?*""
		}
	},
	{
		""type"": ""section"",
		""text"": {
			""type"": ""mrkdwn"",
			""text"": ""Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Aenean lacinia bibendum nulla sed consectetur. Maecenas sed diam eget risus varius blandit sit amet non magna. ""
		}
	},
	{
		""type"": ""section"",
		""text"": {
			""type"": ""mrkdwn"",
			""text"": "" ""
		}
	},
	{
		""type"": ""context"",
		""elements"": [
			{
				""type"": ""mrkdwn"",
				""text"": ""Lion, Kima Ventures, The Family""
			}
		]
	},
	{
		""type"": ""section"",
		""text"": {
			""type"": ""mrkdwn"",
			""text"": ""Great, please respond using thread :speech_balloon:""
		},
		""accessory"": {
			""type"": ""button"",
			""text"": {
				""type"": ""plain_text"",
				""text"": ""Cancel"",
				""emoji"": true
			},
			""style"": ""danger"",
			""value"": ""click_me_123""
		}
	},
	{
		""type"": ""divider""
	},
	{
		""type"": ""context"",
		""elements"": [
			{
				""type"": ""image"",
				""image_url"": ""https://images.pexels.com/photos/220453/pexels-photo-220453.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500"",
				""alt_text"": ""First name Last name | Job @Company""
			},
			{
				""type"": ""image"",
				""image_url"": ""https://images.pexels.com/photos/774909/pexels-photo-774909.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260"",
				""alt_text"": ""First name Last name | Job @Company""
			},
			{
				""type"": ""image"",
				""image_url"": ""https://images.pexels.com/photos/555790/pexels-photo-555790.png?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260"",
				""alt_text"": ""First name Last name | Job @Company""
			},
			{
				""type"": ""image"",
				""image_url"": ""https://images.pexels.com/photos/1172784/pexels-photo-1172784.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500"",
				""alt_text"": ""First name Last name | Job @Company""
			},
			{
				""type"": ""image"",
				""image_url"": ""https://images.pexels.com/photos/1674752/pexels-photo-1674752.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260"",
				""alt_text"": ""First name Last name | Job @Company""
			},
			{
				""type"": ""mrkdwn"",
				""text"": ""and {x} people are insterested!""
			}
		]
	},
	{
		""type"": ""context"",
		""elements"": [
			{
				""type"": ""mrkdwn"",
				""text"": ""Discover what's going on in your communities by going to <https://app.whyse.co/|Whyse>""
			}
		]
	},
	{
		""type"": ""actions"",
		""elements"": [
			{
				""type"": ""button"",
				""text"": {
					""type"": ""plain_text"",
					""text"": ""Yes"",
					""emoji"": true
				}
			},
			{
				""type"": ""button"",
				""text"": {
					""type"": ""plain_text"",
					""text"": ""No"",
					""emoji"": true
				}
			},
			{
				""type"": ""button"",
				""text"": {
					""type"": ""plain_text"",
					""text"": ""Maybe"",
					""emoji"": true
				}
			},
			{
				""type"": ""static_select"",
				""placeholder"": {
					""type"": ""plain_text"",
					""text"": ""Select an item"",
					""emoji"": true
				},
				""options"": [
					{
						""text"": {
							""type"": ""plain_text"",
							""text"": ""Excellent item 1"",
							""emoji"": true
						},
						""value"": ""value-0""
					},
					{
						""text"": {
							""type"": ""plain_text"",
							""text"": ""Fantastic item 2"",
							""emoji"": true
						},
						""value"": ""value-1""
					},
					{
						""text"": {
							""type"": ""plain_text"",
							""text"": ""Nifty item 3"",
							""emoji"": true
						},
						""value"": ""value-2""
					},
					{
						""text"": {
							""type"": ""plain_text"",
							""text"": ""Pretty good item 4"",
							""emoji"": true
						},
						""value"": ""value-3""
					}
				]
			}
		]
	}
]";
			#endregion

			var blocks = deserializer.Deserialize(json).ToList();

			Assert.Equal(12, blocks.Count);
			var actionsBlock = blocks.OfType<ActionsBlock>();
			Assert.NotNull(actionsBlock);

			var sectionActionBlock = blocks[7] as SectionBlock;
			Assert.NotNull(sectionActionBlock);
			Assert.IsType<ButtonElement>(sectionActionBlock.Accessory);
		}
	}
}
