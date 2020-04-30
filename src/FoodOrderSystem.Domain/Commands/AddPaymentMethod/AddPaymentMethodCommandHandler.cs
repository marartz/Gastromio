using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.AddPaymentMethod
{
    public class AddPaymentMethodCommandHandler : ICommandHandler<AddPaymentMethodCommand, PaymentMethodViewModel>
    {
        private readonly IPaymentMethodFactory paymentMethodFactory;
        private readonly IPaymentMethodRepository paymentMethodRepository;

        public AddPaymentMethodCommandHandler(IPaymentMethodFactory paymentMethodFactory, IPaymentMethodRepository paymentMethodRepository)
        {
            this.paymentMethodFactory = paymentMethodFactory;
            this.paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<CommandResult<PaymentMethodViewModel>> HandleAsync(AddPaymentMethodCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult<PaymentMethodViewModel>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult<PaymentMethodViewModel>();

            var paymentMethod = await paymentMethodRepository.FindByNameAsync(command.Name, cancellationToken);
            if (paymentMethod != null)
                return new FailureCommandResult<PaymentMethodViewModel>();

            paymentMethod = paymentMethodFactory.Create(command.Name, command.Description);
            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);

            return new SuccessCommandResult<PaymentMethodViewModel>(PaymentMethodViewModel.FromPaymentMethod(paymentMethod));
        }
    }
}
