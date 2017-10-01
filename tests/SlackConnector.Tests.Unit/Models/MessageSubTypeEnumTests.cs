using System;
using System.Linq;
using Shouldly;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Models;
using Xunit;

namespace SlackConnector.Tests.Unit.Models
{
    public class MessageSubTypeEnumTests
    {
        [Fact]
        public void should_have_same_number_of_enum_values_as_internal_enum_type()
        {
            // given 
            int numberOfInternalEnumValues = Enum.GetNames(typeof(MessageSubType)).Length;
            int numberOfPublicEnumValues = Enum.GetNames(typeof(SlackMessageSubType)).Length;

            // then
            numberOfPublicEnumValues.ShouldBe(numberOfInternalEnumValues);
        }

        [Fact]
        public void should_have_same_message_types_as_internal_model()
        {
            // given 
            var internalEnumValues = Enum.GetNames(typeof(MessageSubType))
                .Select(x => x.Replace("_", string.Empty))
                .Select(x => x.ToLower());
            var publicEnumValues = Enum.GetNames(typeof(SlackMessageSubType))
                .Select(x => x.ToLower());

            // then
            publicEnumValues.All(x => internalEnumValues.Contains(x)).ShouldBeTrue();
        }
    }
}