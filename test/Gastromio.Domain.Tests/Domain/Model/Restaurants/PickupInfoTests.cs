namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class PickupInfoTests
    {
        // [Fact]
        // public void ChangePickupInfo_NotEnabled_SavesPickupInfoAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var pickupInfo = new PickupInfoBuilder()
        //         .WithEnabled(false)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangePickupInfo(pickupInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.PickupInfo.Should().BeEquivalentTo(pickupInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangePickupInfo_Enabled_AverageTimeNoValue_SavesPickupInfoAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var pickupInfo = new PickupInfoBuilder()
        //         .WithEnabled(true)
        //         .WithAverageTime(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangePickupInfo(pickupInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.PickupInfo.Should().BeEquivalentTo(pickupInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangePickupInfo_Enabled_AverageTimeBelowMin_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var pickupInfo = new PickupInfoBuilder()
        //         .WithEnabled(true)
        //         .WithAverageTime(4)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangePickupInfo(pickupInfo, fixture.ChangedBy);
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
        // public void ChangePickupInfo_Enabled_AverageTimeAboveMax_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var pickupInfo = new PickupInfoBuilder()
        //         .WithEnabled(true)
        //         .WithAverageTime(121)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangePickupInfo(pickupInfo, fixture.ChangedBy);
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
        // public void ChangePickupInfo_Enabled_MinimumOrderValueNoValue_SavesPickupInfoAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var pickupInfo = new PickupInfoBuilder()
        //         .WithEnabled(true)
        //         .WithMinimumOrderValue(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangePickupInfo(pickupInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.PickupInfo.Should().BeEquivalentTo(pickupInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangePickupInfo_Enabled_MinimumOrderValueBelowMin_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var pickupInfo = new PickupInfoBuilder()
        //         .WithEnabled(true)
        //         .WithMinimumOrderValue(-0.01m)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangePickupInfo(pickupInfo, fixture.ChangedBy);
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
        // public void ChangePickupInfo_Enabled_MinimumOrderValueAboveMax_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var pickupInfo = new PickupInfoBuilder()
        //         .WithEnabled(true)
        //         .WithMinimumOrderValue(50.01m)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangePickupInfo(pickupInfo, fixture.ChangedBy);
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
        // public void ChangePickupInfo_Enabled_MaximumOrderValueNoValue_SavesPickupInfoAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var pickupInfo = new PickupInfoBuilder()
        //         .WithEnabled(true)
        //         .WithMaximumOrderValue(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangePickupInfo(pickupInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.PickupInfo.Should().BeEquivalentTo(pickupInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangePickupInfo_Enabled_MaximumOrderValueBelowMin_ReturnsFailure()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var pickupInfo = new PickupInfoBuilder()
        //         .WithEnabled(true)
        //         .WithMaximumOrderValue(-0.01m)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangePickupInfo(pickupInfo, fixture.ChangedBy);
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
        // public void ChangePickupInfo_Enabled_AllValid_SavesPickupInfoAndReturnsSuccess()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var pickupInfo = new PickupInfoBuilder()
        //         .WithEnabled(true)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangePickupInfo(pickupInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.PickupInfo.Should().BeEquivalentTo(pickupInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
    }
}
