using Newtonsoft.Json;
using NUnit.Framework;
using Should;
using SlackConnector.Connections.Sockets.Messages.Inbound;
using SlackConnector.Serialising;
using SpecsFor;

namespace SlackConnector.Tests.Unit.Serialising
{
    public static class EnumConverterTests
    {
        internal class given_enum_type_when_checking_can_convert : SpecsFor<EnumConverter>
        {
            private bool _result;

            protected override void When()
            {
                _result = SUT.CanConvert(typeof(TestControl));
            }

            [Test]
            public void then_should_return_true()
            {
                _result.ShouldBeTrue();
            }
        }

        internal class given_non_enum_type_when_checking_can_convert : SpecsFor<EnumConverter>
        {
            private bool _result;

            protected override void When()
            {
                _result = SUT.CanConvert(typeof(EnumConverter));
            }

            [Test]
            public void then_should_return_false()
            {
                _result.ShouldBeFalse();
            }
        }

        internal class given_valid_enum_value_when_converting_to_enum : SpecsFor<EnumConverter>
        {
            private object _result;

            protected override void Given()
            {
                GetMockFor<JsonReader>()
                    .Setup(x => x.Value)
                    .Returns(TestControl.SomethingElse.ToString);
            }

            protected override void When()
            {
                _result = SUT.ReadJson(GetMockFor<JsonReader>().Object, typeof(TestControl), null, null);
            }

            [Test]
            public void then_should_return_enum()
            {
                Assert.That(_result, Is.EqualTo(TestControl.SomethingElse));
            }
        }

        internal class given_valid_enum_with_different_casing_when_converting_to_enum : SpecsFor<EnumConverter>
        {
            private object _result;

            protected override void Given()
            {
                GetMockFor<JsonReader>()
                    .Setup(x => x.Value)
                    .Returns(TestControl.ThirdOption.ToString().ToLower);
            }

            protected override void When()
            {
                _result = SUT.ReadJson(GetMockFor<JsonReader>().Object, typeof(TestControl), null, null);
            }

            [Test]
            public void then_should_return_enum()
            {
                Assert.That(_result, Is.EqualTo(TestControl.ThirdOption));
            }
        }

        internal class given_invalid_enum_when_converting_to_enum : SpecsFor<EnumConverter>
        {
            private object _result;

            protected override void Given()
            {
                GetMockFor<JsonReader>()
                    .Setup(x => x.Value)
                    .Returns("I AM NOT AN ENUM");
            }

            protected override void When()
            {
                _result = SUT.ReadJson(GetMockFor<JsonReader>().Object, typeof(TestControl), null, null);
            }

            [Test]
            public void then_should_return_enum()
            {
                Assert.That(_result, Is.EqualTo(TestControl.Default));
            }
        }

        private enum TestControl
        {
            Default = 0,
            SomethingElse = 1,
            ThirdOption = 2
        }
    }
}