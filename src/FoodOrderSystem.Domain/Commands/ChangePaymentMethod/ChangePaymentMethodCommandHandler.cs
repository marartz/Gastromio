using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.ChangePaymentMethod
{
    public class ChangePaymentMethodCommandHandler : ICommandHandler<ChangePaymentMethodCommand, bool>
    {
        private readonly IPaymentMethodRepository paymentMethodRepository;

        public ChangePaymentMethodCommandHandler(IPaymentMethodRepository paymentMethodRepository)
        {
            this.paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<Result<bool>> HandleAsync(ChangePaymentMethodCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<bool>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<bool>.Forbidden();

            var paymentMethod = await paymentMethodRepository.FindByPaymentMethodIdAsync(command.PaymentMethodId, cancellationToken);
            if (paymentMethod == null)
                return FailureResult<bool>.Create(FailureResultCode.PaymentMethodDoesNotExist);

            var tempResult = paymentMethod.Change(command.Name, command.Description);
            if (tempResult.IsFailure)
                return tempResult;

            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);

            return SuccessResult<bool>.Create(true);
        }
    }
}
