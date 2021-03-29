using Gastromio.Core.Application.Ports;

namespace Gastromio.App
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public bool IsTestSystem { get; set; }
        public string EmailRecipientForTest { get; set; }
        public string MobileRecipientForTest { get; set; }
    }
}