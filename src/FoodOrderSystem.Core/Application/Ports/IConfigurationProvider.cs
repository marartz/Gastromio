namespace FoodOrderSystem.Core.Application.Ports
{
    public interface IConfigurationProvider
    {
        bool IsTestSystem { get; }
        
        string EmailRecipientForTest { get; }
    }
}