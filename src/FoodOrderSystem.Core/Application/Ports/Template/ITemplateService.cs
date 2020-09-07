using FoodOrderSystem.Core.Domain.Model.Order;

namespace FoodOrderSystem.Core.Application.Ports.Template
{
    public interface ITemplateService
    {
        EmailData GetCustomerEmail(Order order);

        EmailData GetRestaurantEmail(Order order);
    }
}