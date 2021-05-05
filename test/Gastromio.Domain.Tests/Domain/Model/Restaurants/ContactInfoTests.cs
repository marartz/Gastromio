namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class ContactInfoTests
    {
        // [Fact]
        // public void ChangeContactInfo_PhoneNull_ReturnsFailure()
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
        // public void ChangeContactInfo_PhoneEmpty_ReturnsFailure()
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
        // public void ChangeContactInfo_PhoneInvalid_ReturnsFailure()
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
        // public void ChangeContactInfo_FaxNull_ChangesContactInfoAndReturnsSuccess()
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
        // public void ChangeContactInfo_FaxEmpty_ChangesContactInfoAndReturnsSuccess()
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
        // public void ChangeContactInfo_FaxInvalid_ReturnsFailure()
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
        // public void ChangeContactInfo_WebSiteNull_ChangesContactInfoAndReturnsSuccess()
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
        // public void ChangeContactInfo_WebSiteEmpty_ChangesContactInfoAndReturnsSuccess()
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
        // public void ChangeContactInfo_WebSiteInvalid_ReturnsFailure()
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
        // public void ChangeContactInfo_EmailAddressNull_ReturnsFailure()
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
        // public void ChangeContactInfo_EmailAddressEmpty_ReturnsFailure()
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
        // public void ChangeContactInfo_EmailAddressInvalid_ReturnsFailure()
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
        // public void ChangeContactInfo_MobileNull_ChangesContactInfoAndReturnsSuccess()
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
        // public void ChangeContactInfo_MobileEmpty_ChangesContactInfoAndReturnsSuccess()
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
        // public void ChangeContactInfo_MobileInvalid_ReturnsFailure()
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
        // public void ChangeContactInfo_AllValid_ChangesContactInfoAndReturnsSuccess()
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
