using FoodOrderSystem.Core.Domain.Model.Order;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Ports.Template
{
    public interface ITemplateService
    {
        EmailData GetCustomerEmail(Order order);

        EmailData GetRestaurantEmail(Order order);

        EmailData GetRequestPasswordChangeEmail(string email, string url);
    }
}