using FoodOrderSystem.Core.Application.Ports;

namespace FoodOrderSystem.App
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public bool IsTestSystem { get; set; }
        public string EmailRecipientForTest { get; set; }
    }
}