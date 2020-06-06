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

        public Task<IEnumerable<PaymentMethod>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                return (IEnumerable<PaymentMethod>) dbContext.PaymentMethods.OrderBy(en => en.Name)
                    .Select(en => FromRow(en));
            }, cancellationToken);
        }

        public Task<PaymentMethod> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var row = dbContext.PaymentMethods.FirstOrDefault(en => en.Name == name);
                if (row == null)
                    return null;
                return FromRow(row);
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

        public Task StoreAsync(PaymentMethod paymentMethod, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.PaymentMethods;

                var row = dbSet.FirstOrDefault(x => x.Id == paymentMethod.Id.Value);
                if (row != null)
                {
                    ToRow(paymentMethod, row);
                    dbSet.Update(row);
                }
                else
                {
                    row = new PaymentMethodRow();
                    ToRow(paymentMethod, row);
                    dbSet.Add(row);
                }

                dbContext.SaveChanges();
            }, cancellationToken);
        }

        public Task RemoveAsync(PaymentMethodId paymentMethodId, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.PaymentMethods;

                var row = dbSet.FirstOrDefault(en => en.Id == paymentMethodId.Value);
                if (row != null)
                {
                    dbSet.Remove(row);
                    dbContext.SaveChanges();
                }
            }, cancellationToken);
        }


        private static PaymentMethod FromRow(PaymentMethodRow row)
        {
            return new PaymentMethod(new PaymentMethodId(row.Id),
                row.Name,
                row.Description
            );
        }

        private static void ToRow(PaymentMethod obj, PaymentMethodRow row)
        {
            row.Id = obj.Id.Value;
            row.Name = obj.Name;
            row.Description = obj.Description;
        }
    }
}
