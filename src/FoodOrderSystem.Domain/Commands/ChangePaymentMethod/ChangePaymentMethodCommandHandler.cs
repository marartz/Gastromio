using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.ChangePaymentMethod
{
    public class ChangePaymentMethodCommandHandler : ICommandHandler<ChangePaymentMethodCommand>
    {
        private readonly IPaymentMethodRepository paymentMethodRepository;

        public ChangePaymentMethodCommandHandler(IPaymentMethodRepository paymentMethodRepository)
        {
            this.paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<CommandResult> HandleAsync(ChangePaymentMethodCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult();

            var paymentMethod = await paymentMethodRepository.FindByPaymentMethodIdAsync(command.PaymentMethodId, cancellationToken);
            if (paymentMethod == null)
                return new FailureCommandResult<string>("user does not exist");

            paymentMethod.Change(command.Name, command.Description);

            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);

            return new SuccessCommandResult<PaymentMethod>(paymentMethod);
        }
    }
}
