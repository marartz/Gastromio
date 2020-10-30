using System;
using System.Collections.Generic;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Domain.Model.Order;
using FoodOrderSystem.Core.Domain.Model.PaymentMethod;

namespace FoodOrderSystem.Core.Application.Commands.Checkout
{
    public class CheckoutCommand : ICommand<OrderDTO>
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
            string restaurantId,
            IList<CartDishInfoDTO> cartDishes,
            string comments,
            PaymentMethodId paymentMethodId,
            DateTime? serviceTime
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
            CartDishes = cartDishes;
            Comments = comments;
            PaymentMethodId = paymentMethodId;
            ServiceTime = serviceTime;
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
        public string RestaurantId { get; }
        public IList<CartDishInfoDTO> CartDishes { get; }
        public string Comments { get; }
        public PaymentMethodId PaymentMethodId { get; }
        public DateTime? ServiceTime { get; }
    }
}