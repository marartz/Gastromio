using System;
using System.IO;
using System.Linq;
using DotLiquid;
using DotLiquid.NamingConventions;
using FoodOrderSystem.Core.Application.Ports.Template;
using FoodOrderSystem.Core.Domain.Model.Order;

namespace FoodOrderSystem.Template.DotLiquid
{
    public class TemplateService : ITemplateService
    {
        public EmailData GetCustomerEmail(Order order)
        {
            var assembly = typeof(TemplateService).Assembly;
            var resourceStream = assembly.GetManifestResourceStream("FoodOrderSystem.Template.DotLiquid.Templates.CustomerTemplate.html");
            if (resourceStream == null)
                throw new InvalidOperationException("cannot read template");

            using (resourceStream)
            using (var streamReader = new StreamReader(resourceStream))
            {
                var customerTemplate = streamReader.ReadToEnd();
                
                global::DotLiquid.Template.NamingConvention = new CSharpNamingConvention();
                var template = global::DotLiquid.Template.Parse(customerTemplate);
            
                var renderResult = template.Render(Hash.FromAnonymousObject(GenerateOrderObject(order)));
            
                return new EmailData
                {
                    Subject = "Ihre Bestellung bei Gastromio.de",
                    TextPart = "",
                    HtmlPart = renderResult
                };
            }
        }

        public EmailData GetRestaurantEmail(Order order)
        {
            var assembly = typeof(TemplateService).Assembly;
            var resourceStream = assembly.GetManifestResourceStream("FoodOrderSystem.Template.DotLiquid.Templates.RestaurantTemplate.html");
            if (resourceStream == null)
                throw new InvalidOperationException("cannot read template");

            using (resourceStream)
            using (var streamReader = new StreamReader(resourceStream))
            {
                var customerTemplate = streamReader.ReadToEnd();
                
                global::DotLiquid.Template.NamingConvention = new CSharpNamingConvention();
                var template = global::DotLiquid.Template.Parse(customerTemplate);
            
                var renderResult = template.Render(Hash.FromAnonymousObject(GenerateOrderObject(order)));

                var customerInfo =
                    $"{order.CustomerInfo.GivenName} {order.CustomerInfo.LastName} ({order.CustomerInfo.Street}, {order.CustomerInfo.ZipCode} {order.CustomerInfo.City})";
            
                return new EmailData
                {
                    Subject = $"Gastromio.de - Neue Bestellung von {customerInfo}",
                    TextPart = "",
                    HtmlPart = renderResult
                };
            }
        }

        private object GenerateOrderObject(Order order)
        {
            return new
            {
                OrderId = order.Id.Value,
                CustomerInfo = new
                {
                    order.CustomerInfo.GivenName,
                    order.CustomerInfo.LastName,
                    order.CustomerInfo.Street,
                    order.CustomerInfo.AddAddressInfo,
                    order.CustomerInfo.ZipCode,
                    order.CustomerInfo.City,
                    order.CustomerInfo.Phone,
                    order.CustomerInfo.Email
                },
                CartInfo = new
                {
                    OrderType = ConvertOrderType(order.CartInfo.OrderType),
                    order.CartInfo.RestaurantName,
                    order.CartInfo.RestaurantInfo,
                    order.CartInfo.RestaurantPhone,
                    order.CartInfo.RestaurantEmail,
                    OrderedDishes = order.CartInfo.OrderedDishes.Select(en =>
                        new
                        {
                            en.DishName,
                            en.VariantName,
                            VariantPrice = en.VariantPrice.ToString("0.00"),
                            en.Count,
                            Price = (en.Count * en.VariantPrice).ToString("0.00"),
                            en.Remarks
                        }
                    )
                },
                order.Comments,
                order.PaymentMethodName,
                order.PaymentMethodDescription,
                Costs = order.Costs.ToString("0.00"),
                TotalPrice = order.TotalPrice.ToString("0.00")
            };
        }

        private static string ConvertOrderType(OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.Pickup:
                    return "Abholung";
                case OrderType.Delivery:
                    return "Lieferung";
                case OrderType.Reservation:
                    return "Reservierung";
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null);
            }
        }
    }
}