namespace Gastromio.Domain.Tests.Domain.Model.Restaurants
{
    public class DeliveryInfoTests
    {
        // [Fact]
        // public void ChangeDeliveryInfo_NotEnabled_SavesDeliveryInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(false)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeliveryInfo.Should().BeEquivalentTo(deliveryInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeDeliveryInfo_Enabled_AverageTimeNoValue_SavesDeliveryInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithAverageTime(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeliveryInfo.Should().BeEquivalentTo(deliveryInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeDeliveryInfo_Enabled_AverageTimeBelowMin_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithAverageTime(4)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
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
        // public void ChangeDeliveryInfo_Enabled_AverageTimeAboveMax_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithAverageTime(121)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
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
        // public void ChangeDeliveryInfo_Enabled_MinimumOrderValueNoValue_SavesDeliveryInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithMinimumOrderValue(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeliveryInfo.Should().BeEquivalentTo(deliveryInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeDeliveryInfo_Enabled_MinimumOrderValueBelowMin_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithMinimumOrderValue(-0.01m)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
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
        // public void ChangeDeliveryInfo_Enabled_MinimumOrderValueAboveMax_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithMinimumOrderValue(50.01m)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
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
        // public void ChangeDeliveryInfo_Enabled_MaximumOrderValueNoValue_SavesDeliveryInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithMaximumOrderValue(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeliveryInfo.Should().BeEquivalentTo(deliveryInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeDeliveryInfo_Enabled_MaximumOrderValueBelowMin_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithMaximumOrderValue(-0.01m)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
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
        // public void ChangeDeliveryInfo_Enabled_AllValid_SavesDeliveryInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeliveryInfo.Should().BeEquivalentTo(deliveryInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeDeliveryInfo_Enabled_CostsNoValue_SavesDeliveryInfo()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithCosts(null)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     var result = testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsSuccess.Should().BeTrue();
        //         testObject.DeliveryInfo.Should().BeEquivalentTo(deliveryInfo);
        //         testObject.UpdatedOn.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        //         testObject.UpdatedBy.Should().Be(fixture.ChangedBy);
        //     }
        // }
        //
        // [Fact]
        // public void ChangeDeliveryInfo_Enabled_CostsBelowMin_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithCosts(-0.01m)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
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
        // public void ChangeDeliveryInfo_Enabled_CostsAboveMax_ThrowsDomainException()
        // {
        //     // Arrange
        //     fixture.SetupChangedBy();
        //     var testObject = fixture.CreateTestObject();
        //
        //     var deliveryInfo = new DeliveryInfoBuilder()
        //         .WithEnabled(true)
        //         .WithCosts(10.01m)
        //         .WithValidConstrains()
        //         .Create();
        //
        //     // Act
        //     Action act = () => testObject.ChangeDeliveryInfo(deliveryInfo, fixture.ChangedBy);
        //
        //     // Assert
        //     using (new AssertionScope())
        //     {
        //         result.Should().NotBeNull();
        //         result?.IsFailure.Should().BeTrue();
        //     }
        // }
    }
}
