using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.GetAllPaymentMethods
{
    public class GetAllPaymentMethodsQueryHandler : IQueryHandler<GetAllPaymentMethodsQuery>
    {
        private readonly IPaymentMethodRepository paymentMethodRepository;

        public GetAllPaymentMethodsQueryHandler(IPaymentMethodRepository paymentMethodRepository)
        {
            this.paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<QueryResult> HandleAsync(GetAllPaymentMethodsQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return new UnauthorizedQueryResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenQueryResult();

            return new SuccessQueryResult<ICollection<PaymentMethod>>(await paymentMethodRepository.FindAllAsync(cancellationToken));
        }
    }
}
