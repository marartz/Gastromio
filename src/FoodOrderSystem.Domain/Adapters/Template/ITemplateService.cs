using FoodOrderSystem.Domain.Model.Order;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Adapters.Template
{
    public interface ITemplateService
    {
        EmailData GetCustomerEmail(Order order);

        EmailData GetRestaurantEmail(Order order);
    }
}