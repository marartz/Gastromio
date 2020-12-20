using System.Text;
using FoodOrderSystem.Core.Application.Ports.Template;
using FoodOrderSystem.Core.Domain.Model.Order;

namespace FoodOrderSystem.Template.DotLiquid
{
    public class TemplateService : ITemplateService
    {
        public EmailData GetCustomerEmail(Order order)
        {
            var sb = new StringBuilder();

            sb.Append("<p>");
            sb.Append("Liebe/r ");
            sb.Append(order.CustomerInfo.GivenName);
            sb.Append(",");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.Append("wir haben Deine Bestellung empfangen und an ");
            sb.Append(order.CartInfo.RestaurantInfo);
            sb.Append(" weitergeleitet!");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.AppendLine(
                "Gastromio.de wurde ehrenamtlich erstellt, um Bocholts schönes Gastronomieangebot zu erhalten. Gastromio wurde");
            sb.AppendLine(
                "erst kürzlich in Betrieb genommen. Es ist also möglich, dass der Wirt noch nicht in der Bestellabwicklung über");
            sb.AppendLine(
                "Gastromio geübt ist, oder sich ein Systemfehler eingeschlichen hat, den wir noch nicht kennen. Wir haben daher");
            sb.AppendLine(
                "den Wirt gebeten, seine Vorbestellungen zu Beginn der Schicht kurz per E-Mail zu bestätigen. Passiert das nicht,");
            sb.AppendLine(
                "frag ruhig kurz nach, sei aber nett, es ist für den Wirt genauso neu, wie für Dich. Das Restaurant ist unter der");
            sb.Append("Telefonnummer ");
            sb.Append(order.CartInfo.RestaurantPhone);
            sb.Append(" zu erreichen.");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.AppendLine(
                "Wenn ein Problem aufgetreten ist, das ihr nicht lösen konntet, melde Dich doch gerne unter support@gastromio.de!");
            sb.Append("Wir nehmen Deinen Hinweis gerne auf.");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.Append("Deine Bestellung bei ");
            sb.Append(order.CartInfo.RestaurantName);
            sb.Append(":");
            sb.Append("</p>");

            AppendOrderDetails(sb, order);

            sb.Append("<p>");
            if (order.CartInfo.OrderType == OrderType.Pickup)
            {
                sb.Append("Die gewählte Bestellart ist: Abholung. Bitte hole das Essen beim Restaurant ab.");
            }
            else if (order.CartInfo.OrderType == OrderType.Delivery)
            {
                sb.Append("Die gewählte Bestellart ist: Lieferung. Das Essen wird Dir nach Hause geliefert.");
            }

            sb.Append("</p>");

            AppendServiceTime(sb, order);

            sb.AppendLine("<br/>");

            sb.Append("<p>");
            sb.AppendLine("Dein Gastromio-Team");
            sb.Append("</p>");

            var message = sb.ToString();

            return new EmailData
            {
                Subject = "Deine Bestellung bei Gastromio.de",
                TextPart = message
                    .Replace("<br/>", "\r\n")
                    .Replace("<p>", "")
                    .Replace("</p>", "\r\n"),
                HtmlPart = message
            };
        }

        public EmailData GetRestaurantEmail(Order order)
        {
            var sb = new StringBuilder();

            sb.Append("<p>");
            sb.Append("Liebes Restaurant ");
            sb.Append(order.CartInfo.RestaurantName);
            sb.Append(",");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.Append("Wir haben eine neue Bestellung für Dich über Gastromio.de entgegengenommen:");
            sb.Append("</p>");

            AppendOrderDetails(sb, order);

            sb.Append("<p>");
            if (order.CartInfo.OrderType == OrderType.Pickup)
            {
                sb.Append("Die gewählte Bestellart ist: Abholung. Der Besteller holt die Bestellung ab.");
            }
            else if (order.CartInfo.OrderType == OrderType.Delivery)
            {
                sb.Append(
                    "Die gewählte Bestellart ist: Lieferung. Bitte dem Besteller die Bestellung zur gewünschten Adresse liefern.");
            }

            sb.Append("</p>");

            AppendServiceTime(sb, order);

            sb.Append("<p>");
            sb.AppendLine("Noch ein wichtiger Hinweis:");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.AppendLine(
                "Gastromio.de wurde ehrenamtlich von Helfern der Coronahilfe-Bocholt erstellt, um Deine Existenz als Gastronom in dieser");
            sb.AppendLine(
                "schweren Zeit zu erhalten. Weder Du noch Dein Kunde haben durch Gastromio.de irgendwelche Kosten. Gastromio.de wurde erst");
            sb.AppendLine(
                "kürzlich in Betrieb genommen. Es ist also möglich, dass der Kunde noch unsicher ist, ob die Bestellabwicklung über Gastromio");
            sb.AppendLine(
                "funktioniert. Wir haben ihm daher zugesagt, dass Du die Bestellung zu Beginn Deiner Schicht kurz per E-Mail bestätigen würdest.");
            sb.Append(
                "Falls er etwas bestellt hat, das aktuell nicht lieferbar ist, rufe ihn bitte an, er ist unter Telefonnummer ");
            sb.AppendLine(order.CustomerInfo.Phone);
            sb.Append("bzw. unter der E-Mail-Adresse ");
            sb.Append(order.CustomerInfo.Email);
            sb.Append(" zu erreichen.");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.AppendLine(
                "Bestellungen per E-Mail betreffen immer nur Vorbestellungen für die nächste Öffnungszeit/Schicht. Bestellungen oder Abholungen,");
            sb.AppendLine(
                "die der Kunde für sofort oder möglichst schnell erhalten will, lassen wir auf Wunsch vieler Wirte, zunächst telefonisch bei Dir");
            sb.Append("eingehen.");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.AppendLine(
                "Wenn ein Problem aufgetreten ist, das Ihr nicht lösen konntet, melde es doch gerne unter support@gastromio.de! Wir nehmen Deinen");
            sb.Append("Hinweis gerne auf.");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.AppendLine(
                "Solltest Du Deine Öffnungszeiten ändern oder Deine Speisekarte anpassen wollen und dabei Schwierigkeiten haben, kannst Du auch");
            sb.AppendLine(
                "gerne bei unserer Hotline nachfragen: 02871-287381 oder aber eine Mail schicken an hotline@coronahilfe-bocholt.de");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.AppendLine(
                "Denke bitte daran: Alle Helfer, die Dein Angebot über Gastromio.de im Internet und auf verschiedenen Sozialen Medien");
            sb.AppendLine(
                "kommunizieren, tun das ehrenamtlich nach bestem Wissen und Gewissen, und völlig ohne Gegenleistung. Oft sind es Stammkunden,");
            sb.AppendLine(
                "die möchten, dass Du weiter existierst, aber vielleicht macht auch einer mal einen Fehler dabei. Bitte sei also nett, wenn Du");
            sb.AppendLine(
                "etwas zu kritisieren hast und erkläre uns einfach, was Du Dir anders wünschst, sie alle tun ihr Bestes.");
            sb.Append("</p>");

            sb.AppendLine("<br/>");

            sb.Append("<p>");
            sb.AppendLine("Dein Gastromio-Team");
            sb.Append("</p>");

            var customerInfo = order.CartInfo.OrderType switch
            {
                OrderType.Delivery =>
                    $"({order.CustomerInfo.GivenName} {order.CustomerInfo.LastName} {order.CustomerInfo.Street}, {order.CustomerInfo.ZipCode} {order.CustomerInfo.City})",
                _ =>
                    $"({order.CustomerInfo.GivenName} {order.CustomerInfo.LastName}, {order.CustomerInfo.Email}, {order.CustomerInfo.Phone})"
            };

            var message = sb.ToString();

            return new EmailData
            {
                Subject = $"Gastromio.de - Neue Bestellung von {customerInfo}",
                TextPart = message
                    .Replace("<br/>", "\r\n")
                    .Replace("<p>", "")
                    .Replace("</p>", "\r\n"),
                HtmlPart = message
            };
        }

        public EmailData GetRequestPasswordChangeEmail(string email, string url)
        {
            var sb = new StringBuilder();

            sb.Append("<p>");
            sb.Append("Hallo ");
            sb.Append(email);
            sb.Append(",");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.Append("wir haben über Gastromio die Aufforderung erhalten, Dein Passwort zurückzusetzen!");
            sb.Append("<br/>");
            sb.Append("Wenn das ein Versehen war oder nicht von Dir veranlasst worden ist, kannst Du diese E-Mail einfach ignorieren. Dein Passwort wurde nicht geändert!");
            sb.Append("</p>");

            sb.Append("<p>");
            sb.AppendLine("Falls Du doch Dein Passwort ändern möchtest, klicke bitte auf den folgenden Link und folge den dort genannten Anweisungen:");
            sb.Append("</p>");

            sb.Append("<a href='");
            sb.Append(url);
            sb.Append("'>");
            sb.Append(url);
            sb.Append("</a>");
            
            sb.AppendLine("<br/>");

            sb.Append("<p>");
            sb.AppendLine("Dein Gastromio-Team");
            sb.Append("</p>");

            var message = sb.ToString();

            return new EmailData
            {
                Subject = $"Gastromio.de - Anforderung zur Änderung Deines Passworts",
                TextPart = message
                    .Replace("<br/>", "\r\n")
                    .Replace("<p>", "")
                    .Replace("</p>", "\r\n"),
                HtmlPart = message
            };
        }

        private static void AppendOrderDetails(StringBuilder sb, Order order)
        {
            sb.Append("<p>");
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

                sb.Append(orderedDish.VariantPrice.ToString("0.00"));
                sb.Append("€");

                if (!string.IsNullOrEmpty(orderedDish.Remarks))
                {
                    sb.Append(" (");
                    sb.Append(orderedDish.Remarks);
                    sb.Append(")");
                }

                sb.AppendLine("<br/>");
            }

            sb.Append("</p>");

            sb.Append("<p>");
            if (order.Costs > 0)
            {
                sb.Append("Lieferkosten: ");
                sb.Append(order.Costs.ToString("0.00"));
                sb.Append("€");
                sb.AppendLine("<br/>");
            }

            sb.Append("Gesamtpreis: ");
            sb.Append(order.TotalPrice.ToString("0.00"));
            sb.Append("€");
            sb.AppendLine("<br/>");

            sb.Append("Zahlungsmethode: ");
            sb.Append(order.PaymentMethodName);
            sb.Append("</p>");

            sb.Append("<p>");
            sb.Append("Bestelldetails:");
            sb.AppendLine("<br/>");

            sb.Append(order.CustomerInfo.GivenName);
            sb.Append(" ");
            sb.Append(order.CustomerInfo.LastName);
            sb.AppendLine("<br/>");

            if (order.CartInfo.OrderType == OrderType.Delivery)
            {
                sb.AppendLine(order.CustomerInfo.Street);
                sb.Append(order.CustomerInfo.ZipCode);
                sb.Append(" ");
                sb.Append(order.CustomerInfo.City);
                sb.AppendLine("<br/>");
            }

            sb.Append("Telefonnummer: ");
            sb.Append(order.CustomerInfo.Phone);
            sb.AppendLine("<br/>");

            sb.Append("E-Mail-Adresse: ");
            sb.Append(order.CustomerInfo.Email);

            if (!string.IsNullOrWhiteSpace(order.CustomerInfo.AddAddressInfo))
            {
                sb.AppendLine("<br/>");
                sb.Append("Zusatzinformationen: ");
                sb.Append(order.CustomerInfo.AddAddressInfo);
            }

            sb.Append("</p>");
        }

        private static void AppendServiceTime(StringBuilder sb, Order order)
        {
            sb.Append("<p>");
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

            sb.Append("</p>");
        }
    }
}