using System;
using FluentAssertions;
using Microsoft.VisualBasic.FileIO;
using Xunit;

namespace Gastromio.Template.DotLiquid.Tests
{
    public class TemplateServiceTests
    {
        [Fact]
        public void GetCustomerEmail_Should_Return_Data_With_Correct_Content()
        {
            // Arrange
            var testFixture = new TestFixture();
            var target = new TemplateService();
            var order = testFixture.GetOrderWithCosts();
            
            // Act
            var emailData = target.GetCustomerEmail(order);
            
            // Assert
            emailData.Should().NotBeNull();
            emailData.Subject.Should().Contain("Ihre Bestellung");
            emailData.HtmlPart.Should().Contain(order.CustomerInfo.GivenName);
            emailData.HtmlPart.Should().Contain(order.CustomerInfo.LastName);
        }
        
        [Fact]
        public void GetRestaurantEmail_Should_Return_Data_With_Correct_Content()
        {
            // Arrange
            var testFixture = new TestFixture();
            var target = new TemplateService();
            var order = testFixture.GetOrderWithCosts();
            
            // Act
            var emailData = target.GetRestaurantEmail(order);
            
            // Assert
            emailData.Should().NotBeNull();
            emailData.Subject.Should().Contain("Neue Bestellung");
            emailData.Subject.Should().Contain(order.CustomerInfo.GivenName);
            emailData.Subject.Should().Contain(order.CustomerInfo.LastName);
            emailData.HtmlPart.Should().Contain(order.CustomerInfo.GivenName);
            emailData.HtmlPart.Should().Contain(order.CustomerInfo.LastName);
        }
    }
}