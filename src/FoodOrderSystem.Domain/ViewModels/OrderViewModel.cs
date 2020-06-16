using System;
using System.Linq;
using FoodOrderSystem.Domain.Model.Order;
using FoodOrderSystem.Domain.Model.PaymentMethod;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        
        public CustomerInfoViewModel CustomerInfo { get; set; }
        
        public CartInfoViewModel CartInfo { get; set; }
        
        public string Comments { get; set; }
        
        public Guid PaymentMethodId { get; set; }
        
        public PaymentMethodViewModel PaymentMethod { get; set; }
        
        public double ValueOfOrder { get; set; }
        
        public double Costs { get; set; }
        
        public double TotalPrice { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public DateTime? UpdatedOn { get; set; }
        
        public Guid? UpdatedBy { get; set; }

        public static OrderViewModel FromOrder(Order order, PaymentMethod paymentMethod)
        {
            return new OrderViewModel
            {
                Id = order.Id.Value,
                CustomerInfo = order.CustomerInfo != null
                    ? new CustomerInfoViewModel
                    {
                        GivenName = order.CustomerInfo.GivenName,
                        LastName = order.CustomerInfo.LastName,
                        Street = order.CustomerInfo.Street,
                        AddAddressInfo = order.CustomerInfo.AddAddressInfo,
                        ZipCode = order.CustomerInfo.ZipCode,
                        City = order.CustomerInfo.City,
                        Phone = order.CustomerInfo.Phone,
                        Email = order.CustomerInfo.Email
                    }
                    : null,
                CartInfo = order.CartInfo != null
                    ? new CartInfoViewModel
                    {
                        OrderType = ConvertOrderType(order.CartInfo.OrderType),
                        RestaurantId = order.CartInfo.RestaurantId.Value,
                        RestaurantInfo = order.CartInfo.RestaurantInfo,
                        OrderedDishes = order.CartInfo.OrderedDishes?.Select(en => new OrderedDishInfoViewModel
                        {
                            ItemId = en.ItemId,
                            Count = en.Count,
                            DishId = en.DishId.Value,
                            DishName = en.DishName,
                            VariantId = en.VariantId,
                            VariantName = en.VariantName,
                            VariantPrice = (double)en.VariantPrice,
                            Remarks = en.Remarks
                        }).ToList()
                    }
                    : null,
                Comments = order.Comments,
                PaymentMethodId = order.PaymentMethodId.Value,
                PaymentMethod = paymentMethod != null
                    ? PaymentMethodViewModel.FromPaymentMethod(paymentMethod)
                    : null,
                ValueOfOrder = (double)order.GetValueOfOrder(),
                Costs = (double)order.Costs,
                TotalPrice = (double)order.GetTotalPrice(),
                CreatedOn = order.CreatedOn,
                UpdatedOn = order.UpdatedOn,
                UpdatedBy = order.UpdatedBy?.Value
            };
        }

        private static string ConvertOrderType(OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.Pickup:
                    return "pickup";
                case OrderType.Delivery:
                    return "delivery";
                case OrderType.Reservation:
                    return "reservation";
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null);
            }
        }
    }
}