using Gastromio.Core.Domain.Model.Order;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Application.Ports.Template
{
    public interface ITemplateService
    {
        EmailData GetCustomerEmail(Order order);

        EmailData GetRestaurantEmail(Order order);

        EmailData GetRequestPasswordChangeEmail(string email, string url);

        string GetRestaurantMobileMessage(Order order);
    }
}