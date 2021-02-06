using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Gastromio.Notification.Sms77.Tests
{
    public class Sms77MobileNotificationServiceTests
    {
        private readonly Fixture fixture;

        public Sms77MobileNotificationServiceTests()
        {
            fixture = new Fixture();
        }

        [Theory]
        [InlineData("015165119020", true, "+4915165119020")]
        [InlineData("0151/65119020", true, "+4915165119020")]
        [InlineData("0151-65119020", true, "+4915165119020")]
        [InlineData("+4915165119020", true, "+4915165119020")]
        [InlineData("004915165119020", true, "+4915165119020")]
        [InlineData("+48123456789", false, null)]
        [InlineData("abc", false, null)]
        public void GetUnifiedPhoneNumber_ProducesAndReturnsCorrectResults(string phoneNumber, bool expectedReturnValue,
            string expectedUnifiedPhoneNumber)
        {
            // Arrange
            fixture.SetupStandardConfiguration();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.GetUnifiedPhoneNumber(phoneNumber, out var unifiedPhoneNumber);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(expectedReturnValue);
                unifiedPhoneNumber.Should().Be(expectedUnifiedPhoneNumber);
            }
        }

        private sealed class Fixture
        {
            public Mock<ILogger<Sms77MobileNotificationService>> LoggerMock;

            public Sms77MobileConfiguration Configuration { get; private set; }

            public Fixture()
            {
                LoggerMock = new Mock<ILogger<Sms77MobileNotificationService>>();
            }

            public Sms77MobileNotificationService CreateTestObject()
            {
                return new Sms77MobileNotificationService(LoggerMock.Object, Configuration);
            }

            public void SetupStandardConfiguration()
            {
                Configuration = new Sms77MobileConfiguration
                {
                    ApiToken = "abc"
                };
            }
        }
    }
}
