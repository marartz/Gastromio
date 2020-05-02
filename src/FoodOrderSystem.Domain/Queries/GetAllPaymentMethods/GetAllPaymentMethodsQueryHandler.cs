using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.GetAllPaymentMethods
{
    public class GetAllPaymentMethodsQueryHandler : IQueryHandler<GetAllPaymentMethodsQuery, ICollection<PaymentMethodViewModel>>
    {
        private readonly IPaymentMethodRepository paymentMethodRepository;

        public GetAllPaymentMethodsQueryHandler(IPaymentMethodRepository paymentMethodRepository)
        {
            this.paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<Result<ICollection<PaymentMethodViewModel>>> HandleAsync(GetAllPaymentMethodsQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var paymentMethods = await paymentMethodRepository.FindAllAsync(cancellationToken);

            return SuccessResult<ICollection<PaymentMethodViewModel>>.Create(paymentMethods.Select(PaymentMethodViewModel.FromPaymentMethod).ToList());
        }
    }
}
