using System;
using FluentAssertions;
using Microsoft.VisualBasic.FileIO;
using Xunit;

namespace FoodOrderSystem.Template.DotLiquid.Tests
{
    public class TemplateServiceTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var testFixture = new TestFixture();
            var target = new TemplateService();
            
            // Act
            var emailData = target.GetCustomerEmail(testFixture.GetOrderWithCosts());
            
            // Assert
            emailData.Should().NotBeNull();
        }
    }
}