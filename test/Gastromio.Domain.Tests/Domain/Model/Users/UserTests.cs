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
    public class UserTests
    {
        public const string ShortestValidPassword = "aB1!!!";

        private readonly Fixture fixture;

        public UserTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void ChangeDetails_ChangesDetails()
        {
            // Arrange
            fixture.SetupChangedBy();
            var testObject = fixture.CreateTestObject();

            const Role role = Role.SystemAdmin;
            var email = "moritz@mustermann.de";

            // Act
            testObject.ChangeDetails(role, email, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.Role.Should().Be(role);
                testObject.Email.Should().Be(email);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ValidatePassword_PasswordMatches_ReturnsTrue()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ValidatePassword(fixture.Password);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ValidatePassword_PasswordDoesNotMatch_ReturnsSuccessWithFalse()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ValidatePassword("abc");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ChangePassword_PasswordNull_ThrowsArgumentNullException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangePassword(null, false, fixture.ChangedBy);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ChangePassword_EmptyPassword_WithoutPasswordPolicy_ChangesPassword()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            var saltBefore = testObject.PasswordSalt;
            var hashBefore = testObject.PasswordHash;

            // Act
            testObject.ChangePassword("", false, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.PasswordSalt.Should().NotBeEquivalentTo(saltBefore);
                testObject.PasswordHash.Should().NotBeEquivalentTo(hashBefore);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangePassword_RandomPassword_WithoutPasswordPolicy_ChangesPassword()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            var saltBefore = testObject.PasswordSalt;
            var hashBefore = testObject.PasswordHash;

            var newPassword = RandomStringBuilder.BuildWithLength(20);

            // Act
            testObject.ChangePassword(newPassword, false, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.PasswordSalt.Should().NotBeEquivalentTo(saltBefore);
                testObject.PasswordHash.Should().NotBeEquivalentTo(hashBefore);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void ChangePassword_EmptyPassword_WithPasswordPolicy_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangePassword("", true, fixture.ChangedBy);

            // Assert
            act.Should().Throw<DomainException<PasswordIsNotValidFailure>>();
        }

        [Fact]
        public void ChangePassword_PasswordTooShort_WithPasswordPolicy_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            const string newPassword = "aA0!";

            // Act
            Action act = () => testObject.ChangePassword(newPassword, true, fixture.ChangedBy);

            // Assert
            act.Should().Throw<DomainException<PasswordIsNotValidFailure>>();
        }

        [Fact]
        public void ChangePassword_PasswordWithoutLowerLetter_WithPasswordPolicy_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            const string newPassword = "AAAABBBB1111!!!!";

            // Act
            Action act = () => testObject.ChangePassword(newPassword, true, fixture.ChangedBy);

            // Assert
            act.Should().Throw<DomainException<PasswordIsNotValidFailure>>();
        }

        [Fact]
        public void ChangePassword_PasswordWithoutUpperLetter_WithPasswordPolicy_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            const string newPassword = "aaaabbbb1111!!!!";

            // Act
            Action act = () => testObject.ChangePassword(newPassword, true, fixture.ChangedBy);

            // Assert
            act.Should().Throw<DomainException<PasswordIsNotValidFailure>>();
        }

        [Fact]
        public void ChangePassword_PasswordWithoutDigit_WithPasswordPolicy_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            const string newPassword = "aaaaBBBBCCCC!!!!";

            // Act
            Action act = () => testObject.ChangePassword(newPassword, true, fixture.ChangedBy);

            // Assert
            act.Should().Throw<DomainException<PasswordIsNotValidFailure>>();
        }

        [Fact]
        public void ChangePassword_PasswordWithoutSpecialCharacter_WithPasswordPolicy_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            const string newPassword = "aaaaBBBB11112222";

            // Act
            Action act = () => testObject.ChangePassword(newPassword, true, fixture.ChangedBy);

            // Assert
            act.Should().Throw<DomainException<PasswordIsNotValidFailure>>();
        }

        [Fact]
        public void ChangePassword_PasswordValidWithLengthOfSix_WithPasswordPolicy_ChangesPassword()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            var testObject = fixture.CreateTestObject();

            var saltBefore = testObject.PasswordSalt;
            var hashBefore = testObject.PasswordHash;

            // Act
            testObject.ChangePassword(ShortestValidPassword, true, fixture.ChangedBy);

            // Assert
            using (new AssertionScope())
            {
                testObject.PasswordSalt.Should().NotBeEquivalentTo(saltBefore);
                testObject.PasswordHash.Should().NotBeEquivalentTo(hashBefore);
                testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
                testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
            }
        }

        [Fact]
        public void GeneratePasswordResetCode_GeneratesCode()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            fixture.SetupRandomResetCodeAndFutureExpiration();
            var testObject = fixture.CreateTestObject();

            var resetCodeBefore = testObject.PasswordResetCode;

            // Act
            testObject.GeneratePasswordResetCode();

            // Assert
            using (new AssertionScope())
            {
                testObject.PasswordResetCode.Should().NotBeEquivalentTo(resetCodeBefore);
                testObject.PasswordResetExpiration.Should().BeCloseTo(DateTimeOffset.UtcNow.AddMinutes(30), 1000);
            }
        }

        [Fact]
        public void ValidatePasswordResetCode_ResetCodeNull_ThrowsArgumentNullException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            fixture.SetupRandomResetCodeAndFutureExpiration();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ValidatePasswordResetCode(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ValidatePasswordResetCode_CurrentResetCodeNotSet_ReturnsFalse()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            fixture.SetupResetCodeAndExpirationNotSet();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ValidatePasswordResetCode(Guid.NewGuid().ToByteArray());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ValidatePasswordResetCode_ExpirationInPast_ReturnsFalse()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            fixture.SetupRandomResetCodeAndPastExpiration();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ValidatePasswordResetCode(fixture.PasswordResetCode);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ValidatePasswordResetCode_ResetCodeDoesNotMatch_ReturnsFalse()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            fixture.SetupRandomResetCodeAndFutureExpiration();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ValidatePasswordResetCode(Guid.NewGuid().ToByteArray());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ValidatePasswordResetCode_ResetCodeMatches_ReturnsTrue()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            fixture.SetupRandomResetCodeAndFutureExpiration();
            var testObject = fixture.CreateTestObject();

            // Act
            var result = testObject.ValidatePasswordResetCode(fixture.PasswordResetCode);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ChangePasswordWithResetCode_ResetCodeNull_ThrowsArgumentNullException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            fixture.SetupRandomResetCodeAndFutureExpiration();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangePasswordWithResetCode(null, ShortestValidPassword);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ChangePasswordWithResetCode_CurrentResetCodeNotSet_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            fixture.SetupResetCodeAndExpirationNotSet();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () =>
                testObject.ChangePasswordWithResetCode(Guid.NewGuid().ToByteArray(), ShortestValidPassword);

            // Assert
            act.Should().Throw<DomainException<PasswordResetCodeIsInvalidFailure>>();
        }

        [Fact]
        public void ChangePasswordWithResetCode_ExpirationInPast_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            fixture.SetupRandomResetCodeAndPastExpiration();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangePasswordWithResetCode(fixture.PasswordResetCode, ShortestValidPassword);

            // Assert
            act.Should().Throw<DomainException<PasswordResetCodeIsInvalidFailure>>();
        }

        [Fact]
        public void ChangePasswordWithResetCode_ResetCodeDoesNotMatch_ThrowsDomainException()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            fixture.SetupRandomResetCodeAndFutureExpiration();
            var testObject = fixture.CreateTestObject();

            // Act
            Action act = () => testObject.ChangePasswordWithResetCode(Guid.NewGuid().ToByteArray(), ShortestValidPassword);

            // Assert
            act.Should().Throw<DomainException<PasswordResetCodeIsInvalidFailure>>();
        }

        [Fact]
        public void ChangePasswordWithResetCode_ResetCodeMatches_ChangesPassword()
        {
            // Arrange
            fixture.SetupChangedBy();
            fixture.SetupRandomPassword();
            fixture.SetupRandomResetCodeAndFutureExpiration();
            var testObject = fixture.CreateTestObject();

            var saltBefore = testObject.PasswordSalt;
            var hashBefore = testObject.PasswordHash;

            // Act
            testObject.ChangePasswordWithResetCode(fixture.PasswordResetCode, ShortestValidPassword);

            // Assert
            using (new AssertionScope())
            {
                testObject.PasswordSalt.Should().NotBeEquivalentTo(saltBefore);
                testObject.PasswordHash.Should().NotBeEquivalentTo(hashBefore);
                testObject.PasswordResetCode.Should().BeNull();
                testObject.PasswordResetExpiration.Should().BeNull();
            }
        }

        private sealed class Fixture
        {
            public UserId ChangedBy { get; private set; }

            public string Password { get; private set; }

            public byte[] PasswordResetCode { get; private set; }

            public DateTimeOffset? PasswordResetExpiration { get; private set; }

            public void SetupChangedBy()
            {
                ChangedBy = new UserIdBuilder().Create();
            }

            public User CreateTestObject()
            {
                var user = new UserBuilder()
                    .WithEmail("max@mustermann.de")
                    .WithPasswordResetCode(PasswordResetCode)
                    .WithPasswordResetExpiration(PasswordResetExpiration)
                    .Create();

                if (!string.IsNullOrEmpty(Password))
                {
                    user.ChangePassword(Password, false, new UserIdBuilder().Create());
                }

                return user;
            }

            public void SetupRandomPassword()
            {
                Password = RandomStringBuilder.BuildWithLength(20);
            }

            public void SetupResetCodeAndExpirationNotSet()
            {
                PasswordResetCode = null;
                PasswordResetExpiration = null;
            }

            public void SetupRandomResetCodeAndFutureExpiration()
            {
                PasswordResetCode = Guid.NewGuid().ToByteArray();
                PasswordResetExpiration = DateTimeOffset.UtcNow.AddMinutes(30);
            }

            public void SetupRandomResetCodeAndPastExpiration()
            {
                PasswordResetCode = Guid.NewGuid().ToByteArray();
                PasswordResetExpiration = DateTimeOffset.UtcNow.AddMinutes(-30);
            }
        }
    }
}
