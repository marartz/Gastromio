using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.RemovePaymentMethod
{
    public class RemovePaymentMethodCommandHandler : ICommandHandler<RemovePaymentMethodCommand, bool>
    {
        private readonly IPaymentMethodRepository paymentMethodRepository;

        public RemovePaymentMethodCommandHandler(IPaymentMethodRepository paymentMethodRepository)
        {
            this.paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<CommandResult<bool>> HandleAsync(RemovePaymentMethodCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult<bool>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult<bool>();

            await paymentMethodRepository.RemoveAsync(command.PaymentMethodId, cancellationToken);

            return new SuccessCommandResult<bool>(true);
        }
    }
}
