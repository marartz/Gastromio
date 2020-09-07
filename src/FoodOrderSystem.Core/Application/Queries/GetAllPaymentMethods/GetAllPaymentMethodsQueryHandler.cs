using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.PaymentMethod;
using FoodOrderSystem.Core.Domain.Model.User;


namespace FoodOrderSystem.Core.Application.Queries.GetAllPaymentMethods
{
    public class GetAllPaymentMethodsQueryHandler : IQueryHandler<GetAllPaymentMethodsQuery, ICollection<PaymentMethodDTO>>
    {
        private readonly IPaymentMethodRepository paymentMethodRepository;

        public GetAllPaymentMethodsQueryHandler(IPaymentMethodRepository paymentMethodRepository)
        {
            this.paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<Result<ICollection<PaymentMethodDTO>>> HandleAsync(GetAllPaymentMethodsQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var paymentMethods = await paymentMethodRepository.FindAllAsync(cancellationToken);

            return SuccessResult<ICollection<PaymentMethodDTO>>.Create(paymentMethods.Select(en => new PaymentMethodDTO(en)).ToList());
        }
    }
}
