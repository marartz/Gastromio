using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Core.Domain.Model.PaymentMethod
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private static List<PaymentMethod> paymentMethods = new List<PaymentMethod>
        {
            new PaymentMethod(new PaymentMethodId(Guid.Parse("8DBBC822-E4FF-47B6-8CA2-68F4F0C51AA3")),
                "Bar", "Sie bezahlen mit Bargeld", "bar"),
            new PaymentMethod(new PaymentMethodId(Guid.Parse("146CEA98-B5FE-45E1-AF65-E42E22A0946F")),
                "EC-Karte", "Sie bezahlen mit Ihrer EC-Karte", "maestro"),
            new PaymentMethod(new PaymentMethodId(Guid.Parse("8ACAAEAF-9AE3-41EC-BC5C-8D0333763B78")),
                "Kreditkarte", "Sie bezahlen mit Ihrer Kreditkarte", "kreditkarte"),
            new PaymentMethod(new PaymentMethodId(Guid.Parse("6B784F68-F912-4754-80E6-F11E3E9FAA40")),
                "PayPal", "Sie bezahlen mit PayPal", "paypal"),
            new PaymentMethod(new PaymentMethodId(Guid.Parse("64951B66-C4A9-4EE0-A4D9-EA44110B178E")),
                "Rechnung", "Sie bezahlen später auf Rechnung", "rechnung")
        };
        
        public Task<IEnumerable<PaymentMethod>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(paymentMethods.AsEnumerable());
        }

        public Task<PaymentMethod> FindByNameAsync(string name, bool ignoreCase, CancellationToken cancellationToken = default)
        {
            var paymentMethod = paymentMethods.FirstOrDefault(pm =>
                string.Equals(pm.Name, name, StringComparison.InvariantCultureIgnoreCase));
            return Task.FromResult(paymentMethod);
        }

        public Task<PaymentMethod> FindByPaymentMethodIdAsync(PaymentMethodId paymentMethodId, CancellationToken cancellationToken = default)
        {
            var paymentMethod = paymentMethods.FirstOrDefault(pm => pm.Id == paymentMethodId);
            return Task.FromResult(paymentMethod);
        }
    }
}