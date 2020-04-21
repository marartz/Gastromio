using FoodOrderSystem.Domain.Model.PaymentMethod;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Persistence
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly SystemDbContext dbContext;

        public PaymentMethodRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<ICollection<PaymentMethod>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                return (ICollection<PaymentMethod>)dbContext.PaymentMethods.Select(en => FromRow(en)).ToList();
            }, cancellationToken);
        }

        public Task<PaymentMethod> FindByPaymentMethodIdAsync(PaymentMethodId paymentMethodId, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var row = dbContext.PaymentMethods.FirstOrDefault(en => en.Id == paymentMethodId.Value);
                if (row == null)
                    return null;
                return FromRow(row);
            }, cancellationToken);
        }

        private static PaymentMethod FromRow(PaymentMethodRow row)
        {
            return new PaymentMethod(new PaymentMethodId(row.Id),
                row.Name,
                row.Description
            );
        }

        private static PaymentMethodRow ToRow(PaymentMethod obj)
        {
            return new PaymentMethodRow
            {
                Id = obj.Id.Value,
                Name = obj.Name,
                Description = obj.Description
            };
        }
    }
}
