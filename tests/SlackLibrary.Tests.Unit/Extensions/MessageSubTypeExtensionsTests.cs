using Shouldly;
using SlackLibrary.Connections.Sockets.Messages.Inbound;
using SlackLibrary.Extensions;
using SlackLibrary.Models;
using Xunit;

namespace SlackLibrary.Tests.Unit.Extensions
{
    public class MessageSubTypeExtensionsTests
    {
        [Theory]
        [InlineData(MessageSubType.bot_message, SlackMessageSubType.BotMessage)]
        [InlineData(MessageSubType.channel_name, SlackMessageSubType.ChannelName)]
        [InlineData(MessageSubType.channel_topic, SlackMessageSubType.ChannelTopic)]
        [InlineData(MessageSubType.group_join, SlackMessageSubType.GroupJoin)]
        private void should_convert_to_expected_enum(MessageSubType inbound, SlackMessageSubType expected)
        {
            inbound
                .ToSlackMessageSubType()
                .ShouldBe(expected);
        }
    }
}