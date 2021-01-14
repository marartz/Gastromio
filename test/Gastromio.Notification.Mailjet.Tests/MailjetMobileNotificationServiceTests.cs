using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Gastromio.Notification.Mailjet.Tests
{
    public class MailjetMobileNotificationServiceTests
    {
        private readonly Fixture fixture;

        public MailjetMobileNotificationServiceTests()
        {
            fixture = new Fixture();
        }

        [Theory]
        [InlineData("015165119020", true, "+4915165119020")]
        [InlineData("+4915165119020", true, "+4915165119020")]
        [InlineData("004915165119020", true, "+4915165119020")]
        [InlineData("+48123456789", true, null)]
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
            public Mock<ILogger<MailjetMobileNotificationService>> LoggerMock;

            public MailjetMobileConfiguration Configuration { get; private set; }

            public Fixture()
            {
                LoggerMock = new Mock<ILogger<MailjetMobileNotificationService>>();
            }

            public MailjetMobileNotificationService CreateTestObject()
            {
                return new MailjetMobileNotificationService(LoggerMock.Object, Configuration);
            }

            public void SetupStandardConfiguration()
            {
                Configuration = new MailjetMobileConfiguration
                {
                    ApiToken = "abc"
                };
            }
        }
    }
}
