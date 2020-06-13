using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.Order;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.Checkout
{
    public class CheckoutCommand : ICommand<OrderId>
    {
        public CheckoutCommand(
            string givenName,
            string lastName,
            string street,
            string addAddressInfo,
            string zipCode,
            string city,
            string phone,
            string email,
            OrderType orderType,
            RestaurantId restaurantId,
            IList<OrderedDishInfo> orderedDishes,
            string comments
        )
        {
            GivenName = givenName;
            LastName = lastName;
            Street = street;
            AddAddressInfo = addAddressInfo;
            ZipCode = zipCode;
            City = city;
            Phone = phone;
            Email = email;
            OrderType = orderType;
            RestaurantId = restaurantId;
            OrderedDishes = orderedDishes;
            Comments = comments;
        }
        
        public string GivenName { get; }
        public string LastName { get; }
        public string Street { get; }
        public string AddAddressInfo { get; }
        public string ZipCode { get; }
        public string City { get; }
        public string Phone { get; }
        public string Email { get; }
        public OrderType OrderType { get; }
        public RestaurantId RestaurantId { get; }
        public IList<OrderedDishInfo> OrderedDishes { get; }
        public string Comments { get; }
    }
}