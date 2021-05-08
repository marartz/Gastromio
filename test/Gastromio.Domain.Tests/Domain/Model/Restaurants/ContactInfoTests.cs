namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class ContactInfoTests
    {
        // [Fact]
        // public void ChangeContactInfo_PhoneNull_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithPhone(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_PhoneEmpty_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithPhone("")
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_PhoneInvalid_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithPhone("abcd")
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_FaxNull_ChangesContactInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithFax(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_FaxEmpty_ChangesContactInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithFax("")
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_FaxInvalid_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithFax("abcd")
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_WebSiteNull_ChangesContactInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithWebSite(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_WebSiteEmpty_ChangesContactInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithWebSite("")
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_WebSiteInvalid_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithWebSite("abcd")
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_EmailAddressNull_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithEmailAddress(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_EmailAddressEmpty_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithEmailAddress("")
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_EmailAddressInvalid_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithEmailAddress("abcd")
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_MobileNull_ChangesContactInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithMobile(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_MobileEmpty_ChangesContactInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithMobile("")
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_MobileInvalid_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithMobile("abcd")
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
        //
        // [Fact]
        // public void ChangeContactInfo_AllValid_ChangesContactInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var contactInfo = new ContactInfoBuilder()
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeContactInfo(contactInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.ContactInfo.Should().BeEquivalentTo(contactInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }

    }
}
