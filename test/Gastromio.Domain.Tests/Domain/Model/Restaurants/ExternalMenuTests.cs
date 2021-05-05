namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class ExternalMenuTests
    {
        // [Fact]
        // public void SetExternalMenu_ExternalMenuNull_ThrowsArgumentNullException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyExternalMenus();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     Action act = () => testObject.SetExternalMenu(null, fixture.ChangedBy);
        //
        //     // Assert
        //     act.Should().Throw<ArgumentNullException>();
        // }
        //
        // [Fact]
        // public void SetExternalMenu_NameNull_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyExternalMenus();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var externalMenu = new ExternalMenuBuilder()
        //         .WithName(null)
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.SetExternalMenu(externalMenu, fixture.ChangedBy);
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
        // public void SetExternalMenu_NameEmpty_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyExternalMenus();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var externalMenu = new ExternalMenuBuilder()
        //         .WithName("")
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.SetExternalMenu(externalMenu, fixture.ChangedBy);
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
        // public void SetExternalMenu_DescriptionNull_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyExternalMenus();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var externalMenu = new ExternalMenuBuilder()
        //         .WithDescription(null)
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.SetExternalMenu(externalMenu, fixture.ChangedBy);
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
        // public void SetExternalMenu_DescriptionEmpty_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyExternalMenus();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var externalMenu = new ExternalMenuBuilder()
        //         .WithDescription("")
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.SetExternalMenu(externalMenu, fixture.ChangedBy);
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
        // public void SetExternalMenu_UrlNull_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyExternalMenus();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var externalMenu = new ExternalMenuBuilder()
        //         .WithUrl(null)
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.SetExternalMenu(externalMenu, fixture.ChangedBy);
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
        // public void SetExternalMenu_UrlEmpty_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyExternalMenus();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var externalMenu = new ExternalMenuBuilder()
        //         .WithUrl("")
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.SetExternalMenu(externalMenu, fixture.ChangedBy);
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
        // public void SetExternalMenu_ExternalMenuNotKnown_AddsExternalMenuAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyExternalMenus();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var newExternalMenu = new ExternalMenuBuilder()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.SetExternalMenu(newExternalMenu, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.ExternalMenus.Should().BeEquivalentTo(newExternalMenu);
        //     }
        // }
        //
        // [Fact]
        // public void SetExternalMenu_ExternalMenuKnown_ReplacesExternalMenuAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupOneExternalMenu();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var newExternalMenu = new ExternalMenuBuilder()
        //         .WithId(fixture.ExternalMenu.Id)
        //         .Create();
        //
        //     // Act
        //     var result = testObject.SetExternalMenu(newExternalMenu, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.ExternalMenus.Should().BeEquivalentTo(newExternalMenu);
        //     }
        // }
        //

        // [Fact]
        // public void RemoveExternalMenu_ExternalMenuNull_ThrowsArgumentException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyExternalMenus();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     Action act = () => testObject.RemoveExternalMenu(Guid.Empty, fixture.ChangedBy);
        //
        //     // Assert
        //     act.Should().Throw<ArgumentException>();
        // }
        //
        // [Fact]
        // public void RemoveExternalMenu_ExternalMenuNotKnown_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupEmptyExternalMenus();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     Action act = () => testObject.RemoveExternalMenu(Guid.NewGuid(), fixture.ChangedBy);
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
        // public void RemoveExternalMenu_ExternalMenuKnown_RemovesExternalMenuAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     fixture.SetupOneExternalMenu();
        //     var testObject = fixture.CreateTestObject();
        //
        //     // Act
        //     var result = testObject.RemoveExternalMenu(fixture.ExternalMenu.Id, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.ExternalMenus.Should().BeEmpty();
        //     }
        // }
    }
}
