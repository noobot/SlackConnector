using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using Should;
using SlackConnector.Serialising;

namespace SlackConnector.Tests.Unit.Serialising
{
    internal class EnumConverterTests
    {
        [Test, AutoMoqData]
        public void should_return_true_given_enum_type_when_checking_can_convert(EnumConverter converter)
        {
            // given

            // when
            var result = converter.CanConvert(typeof(TestControl));

            // then
            result.ShouldBeTrue();
        }

        [Test, AutoMoqData]
        public void should_return_false_given_non_enum_type_when_checking_can_convert(EnumConverter converter)
        {
            // given

            // when
            var result = converter.CanConvert(typeof(EnumConverter));

            // then
            result.ShouldBeFalse();
        }

        [Test, AutoMoqData]
        public void should_ruturn_enum_given_valid_enum_value_when_converting_to_enum([Frozen]Mock<JsonReader> jsonReader, EnumConverter converter)
        {
            // given
            jsonReader
                .Setup(x => x.Value)
                .Returns(TestControl.SomethingElse.ToString);

            // when
            var result = converter.ReadJson(jsonReader.Object, typeof(TestControl), null, null);

            // then
            Assert.That(result, Is.EqualTo(TestControl.SomethingElse));
        }

        [Test, AutoMoqData]
        public void should_ruturn_enum_given_valid_enum_with_different_casing_when_converting_to_enum([Frozen]Mock<JsonReader> jsonReader, EnumConverter converter)
        {
            // given
            jsonReader
                .Setup(x => x.Value)
                .Returns(TestControl.ThirdOption.ToString().ToLower);

            // when
            var result = converter.ReadJson(jsonReader.Object, typeof(TestControl), null, null);

            // then
            Assert.That(result, Is.EqualTo(TestControl.ThirdOption));
        }

        [Test, AutoMoqData]
        public void should_return_default_enum_given_invalid_enum_when_converting_to_enum([Frozen]Mock<JsonReader> jsonReader, EnumConverter converter)
        {
            // given
            jsonReader
                .Setup(x => x.Value)
                .Returns("I AM NOT AN ENUM");

            // when
            var result = converter.ReadJson(jsonReader.Object, typeof(TestControl), null, null);

            // then
            Assert.That(result, Is.EqualTo(TestControl.Default));
        }
        
        private enum TestControl
        {
            Default = 0,
            SomethingElse = 1,
            ThirdOption = 2
        }
    }
}