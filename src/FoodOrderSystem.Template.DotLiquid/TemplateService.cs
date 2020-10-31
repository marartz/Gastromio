using System.Text;
using FoodOrderSystem.Core.Application.Ports.Template;
using FoodOrderSystem.Core.Domain.Model.Order;

namespace FoodOrderSystem.Template.DotLiquid
{
    public class TemplateService : ITemplateService
    {
        public EmailData GetCustomerEmail(Order order)
        {
            // var assembly = typeof(TemplateService).Assembly;
            // var resourceStream = assembly.GetManifestResourceStream("FoodOrderSystem.Template.DotLiquid.Templates.CustomerTemplate.html");
            // if (resourceStream == null)
            //     throw new InvalidOperationException("cannot read template");
            //
            // using (resourceStream)
            // using (var streamReader = new StreamReader(resourceStream))
            // {
            //     var customerTemplate = streamReader.ReadToEnd();
            //     
            //     global::DotLiquid.Template.NamingConvention = new CSharpNamingConvention();
            //     var template = global::DotLiquid.Template.Parse(customerTemplate);
            //
            //     var renderResult = template.Render(Hash.FromAnonymousObject(GenerateOrderObject(order)));
            //
            //     return new EmailData
            //     {
            //         Subject = "Ihre Bestellung bei Gastromio.de",
            //         TextPart = "",
            //         HtmlPart = renderResult
            //     };
            // }

            var sb = new StringBuilder();

            sb.Append("Hallo ");
            sb.Append(order.CustomerInfo.GivenName);
            sb.Append(",");
            sb.AppendLine();
            sb.AppendLine();

            sb.Append("wir haben Deine Bestellung empfangen und an ");
            sb.Append(order.CartInfo.RestaurantInfo);
            sb.Append(" weitergeleitet!");
            sb.AppendLine();
            sb.AppendLine();

            sb.Append(
                "Bei Fragen oder Anmerkungen zu Deiner getätigten Bestellung möchten wir Dich bitten, das Restaurant unter ");
            sb.Append(order.CartInfo.RestaurantPhone);
            sb.Append(" anzurufen.");
            sb.AppendLine();
            sb.AppendLine();

            sb.Append("Deine Bestellung bei ");
            sb.Append(order.CartInfo.RestaurantName);
            sb.Append(":");
            sb.AppendLine();
            sb.AppendLine();

            AppendOrderDetails(sb, order);

            if (order.CartInfo.OrderType == OrderType.Pickup)
            {
                sb.Append("Die gewählte Bestellart ist: Abholung. Bitte hole das Essen beim Restaurant ab.");
                sb.AppendLine();
            }
            else if (order.CartInfo.OrderType == OrderType.Delivery)
            {
                sb.Append("Die gewählte Bestellart ist: Lieferung. Das Essen wird Dir nach Hause geliefert.");
                sb.AppendLine();
            }
            
            AppendServiceTime(sb, order);
            
            sb.AppendLine();

            sb.AppendLine("Dein Gastromio-Team");

            return new EmailData
            {
                Subject = "Ihre Bestellung bei Gastromio.de",
                TextPart = sb.ToString(),
                HtmlPart = ""
            };
        }

        public EmailData GetRestaurantEmail(Order order)
        {
            // var assembly = typeof(TemplateService).Assembly;
            // var resourceStream = assembly.GetManifestResourceStream("FoodOrderSystem.Template.DotLiquid.Templates.RestaurantTemplate.html");
            // if (resourceStream == null)
            //     throw new InvalidOperationException("cannot read template");
            //
            // using (resourceStream)
            // using (var streamReader = new StreamReader(resourceStream))
            // {
            //     var customerTemplate = streamReader.ReadToEnd();
            //     
            //     global::DotLiquid.Template.NamingConvention = new CSharpNamingConvention();
            //     var template = global::DotLiquid.Template.Parse(customerTemplate);
            //
            //     var renderResult = template.Render(Hash.FromAnonymousObject(GenerateOrderObject(order)));
            //
            //     var customerInfo =
            //         $"{order.CustomerInfo.GivenName} {order.CustomerInfo.LastName} ({order.CustomerInfo.Street}, {order.CustomerInfo.ZipCode} {order.CustomerInfo.City})";
            //
            //     return new EmailData
            //     {
            //         Subject = $"Gastromio.de - Neue Bestellung von {customerInfo}",
            //         TextPart = "",
            //         HtmlPart = renderResult
            //     };
            // }

            var sb = new StringBuilder();

            sb.Append("Hallo ");
            sb.Append(order.CartInfo.RestaurantName);
            sb.Append(",");
            sb.AppendLine();
            sb.AppendLine();

            sb.Append("Wir haben eine neue Bestellung empfangen. Hier die Details:");
            sb.AppendLine();
            sb.AppendLine();

            AppendOrderDetails(sb, order);

            if (order.CartInfo.OrderType == OrderType.Pickup)
            {
                sb.Append("Die gewählte Bestellart ist: Abholung. Der Besteller holt die Bestellung ab.");
                sb.AppendLine();
            }
            else if (order.CartInfo.OrderType == OrderType.Delivery)
            {
                sb.Append("Die gewählte Bestellart ist: Lieferung. Bitte dem Besteller die Bestellung zur gewünschten Adresse liefern.");
                sb.AppendLine();
            }
            
            AppendServiceTime(sb, order);
            
            sb.AppendLine();

            sb.AppendLine("Dein Gastromio-Team");

            var customerInfo = order.CartInfo.OrderType switch
            {
                OrderType.Delivery =>
                    $"({order.CustomerInfo.GivenName} {order.CustomerInfo.LastName} {order.CustomerInfo.Street}, {order.CustomerInfo.ZipCode} {order.CustomerInfo.City})",
                _ => $"({order.CustomerInfo.GivenName} {order.CustomerInfo.LastName}, {order.CustomerInfo.Email}, {order.CustomerInfo.Phone})"
            };
                
            return new EmailData
            {
                Subject = $"Gastromio.de - Neue Bestellung von {customerInfo}",
                TextPart = sb.ToString(),
                HtmlPart = ""
            };
        }

        private static void AppendOrderDetails(StringBuilder sb, Order order)
        {
            foreach (var orderedDish in order.CartInfo.OrderedDishes)
            {
                sb.Append(orderedDish.Count);
                sb.Append("x ");

                sb.Append(orderedDish.DishName);

                if (!string.IsNullOrEmpty(orderedDish.VariantName))
                {
                    sb.Append(" (");
                    sb.Append(orderedDish.VariantName);
                    sb.Append(")");
                }

                sb.Append(": ");

                sb.Append(orderedDish.VariantPrice);
                sb.Append("€");

                if (!string.IsNullOrEmpty(orderedDish.Remarks))
                {
                    sb.Append(" (");
                    sb.Append(orderedDish.Remarks);
                    sb.Append(" )");
                }

                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine();

            if (order.Costs > 0)
            {
                sb.Append("Lieferkosten: ");
                sb.Append(order.Costs);
                sb.Append("€");
                sb.AppendLine();
            }

            sb.Append("Gesamtpreis: ");
            sb.Append(order.TotalPrice);
            sb.Append("€");
            sb.AppendLine();

            sb.Append("Zahlungsmethode: ");
            sb.Append(order.PaymentMethodName);
            sb.AppendLine();

            sb.AppendLine();

            sb.Append("Bestelldetails:");
            sb.AppendLine();

            sb.Append(order.CustomerInfo.GivenName);
            sb.Append(" ");
            sb.Append(order.CustomerInfo.LastName);
            sb.AppendLine();

            if (order.CartInfo.OrderType == OrderType.Delivery)
            {
                sb.AppendLine(order.CustomerInfo.Street);
                sb.Append(order.CustomerInfo.ZipCode);
                sb.Append(" ");
                sb.Append(order.CustomerInfo.City);
                sb.AppendLine();
            }

            sb.Append("Telefonnummer: ");
            sb.Append(order.CustomerInfo.Phone);
            sb.AppendLine();

            sb.Append("E-Mail Adresse: ");
            sb.Append(order.CustomerInfo.Email);
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(order.CustomerInfo.AddAddressInfo))
            {
                sb.Append("Zusatzinformationen: ");
                sb.Append(order.CustomerInfo.AddAddressInfo);
                sb.AppendLine();
            }
            
            sb.AppendLine();
        }

        private static void AppendServiceTime(StringBuilder sb, Order order)
        {
            sb.Append("Gewünschtes Datum / Uhrzeit: ");
            if (order.ServiceTime.HasValue)
            {
                var localServiceTime = order.ServiceTime.Value.ToLocalTime();
                sb.Append(localServiceTime.ToLongDateString());
                sb.Append(" ");
                sb.Append(localServiceTime.ToLongTimeString());
            }
            else
            {
                sb.Append("Schnellstmöglich");
            }

            sb.AppendLine();
        }

        // private object GenerateOrderObject(Order order)
        // {
        //     return new
        //     {
        //         OrderId = order.Id.Value,
        //         CustomerInfo = new
        //         {
        //             order.CustomerInfo.GivenName,
        //             order.CustomerInfo.LastName,
        //             order.CustomerInfo.Street,
        //             order.CustomerInfo.AddAddressInfo,
        //             order.CustomerInfo.ZipCode,
        //             order.CustomerInfo.City,
        //             order.CustomerInfo.Phone,
        //             order.CustomerInfo.Email
        //         },
        //         CartInfo = new
        //         {
        //             OrderType = ConvertOrderType(order.CartInfo.OrderType),
        //             order.CartInfo.RestaurantName,
        //             order.CartInfo.RestaurantInfo,
        //             order.CartInfo.RestaurantPhone,
        //             order.CartInfo.RestaurantEmail,
        //             OrderedDishes = order.CartInfo.OrderedDishes.Select(en =>
        //                 new
        //                 {
        //                     en.DishName,
        //                     en.VariantName,
        //                     VariantPrice = en.VariantPrice.ToString("0.00"),
        //                     en.Count,
        //                     Price = (en.Count * en.VariantPrice).ToString("0.00"),
        //                     en.Remarks
        //                 }
        //             )
        //         },
        //         order.Comments,
        //         order.PaymentMethodName,
        //         order.PaymentMethodDescription,
        //         Costs = order.Costs.ToString("0.00"),
        //         TotalPrice = order.TotalPrice.ToString("0.00")
        //     };
        // }

        // private static string ConvertOrderType(OrderType orderType)
        // {
        //     switch (orderType)
        //     {
        //         case OrderType.Pickup:
        //             return "Abholung";
        //         case OrderType.Delivery:
        //             return "Lieferung";
        //         case OrderType.Reservation:
        //             return "Reservierung";
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null);
        //     }
        // }
    }
}