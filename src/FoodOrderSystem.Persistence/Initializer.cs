using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderSystem.Persistence
{
    public static class Initializer
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddTransient<IRestaurantRepository, RestaurantRepository>();
            services.AddTransient<IDishCategoryRepository, DishCategoryRepository>();
        }
    }
}
