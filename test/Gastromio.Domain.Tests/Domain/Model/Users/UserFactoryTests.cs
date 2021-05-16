using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Common;
using Gastromio.Domain.TestKit.Domain.Model.Users;
using Xunit;

namespace Gastromio.Domain.Tests.Domain.Model.Users
{
    public class UserFactoryTests
    {
        private readonly Fixture fixture;

        public UserFactoryTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Create_ValidParameters_CreatesUser()
        {
            // Arrange
            fixture.SetupValidParameters();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.Create(
                fixture.Role,
                fixture.Email,
                fixture.Password,
                fixture.CheckPasswordPolicy,
                fixture.CreatedBy
            );

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result?.Should().BeOfType<User>();
                result?.Id.Value.Should().NotBeEmpty();
                result?.Role.Should().Be(fixture.Role);
                result?.Email.Should().Be(fixture.Email);
                result?.PasswordSalt.Should().NotBeNull();
                result?.PasswordHash.Should().NotBeNull();
                result?.PasswordResetCode.Should().BeNull();
                result?.PasswordResetExpiration.Should().BeNull();
                result?.CreatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                result?.CreatedBy.Should().Be(fixture.CreatedBy);
                result?.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                result?.UpdatedBy.Should().Be(fixture.CreatedBy);
            }
        }

        [Fact]
        public void Create_ValidParametersExceptPassword_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupValidParametersExceptPassword();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.Create(
                fixture.Role,
                fixture.Email,
                fixture.Password,
                fixture.CheckPasswordPolicy,
                fixture.CreatedBy
            );

            // Assert
            act.Should().Throw<DomainException<PasswordIsNotValidFailure>>();
        }

        private sealed class Fixture
        {
            public Role Role { get; private set; }

            public string Email { get; private set; }

            public string Password { get; private set; }

            public bool CheckPasswordPolicy { get; private set; }

            public UserId CreatedBy { get; private set; }

            public void SetupValidParameters()
            {
                Role = Role.SystemAdmin;
                Email = "max@mustermann.de";
                Password = UserTests.ShortestValidPassword;
                CheckPasswordPolicy = true;
                CreatedBy = new UserIdBuilder().Create();
            }

            public void SetupValidParametersExceptPassword()
            {
                SetupValidParameters();
                Password = "abc";
            }

            public UserFactory CreateTestObject()
            {
                return new UserFactory();
            }
        }
    }
}
