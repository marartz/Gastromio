using FoodOrderSystem.Domain.Model;
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

        public async Task<Result<PaymentMethodViewModel>> HandleAsync(AddPaymentMethodCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<PaymentMethodViewModel>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<PaymentMethodViewModel>.Forbidden();

            var paymentMethod = await paymentMethodRepository.FindByNameAsync(command.Name, cancellationToken);
            if (paymentMethod != null)
                return FailureResult<PaymentMethodViewModel>.Create(FailureResultCode.PaymentMethodAlreadyExists);

            paymentMethod = paymentMethodFactory.Create(command.Name, command.Description);
            await paymentMethodRepository.StoreAsync(paymentMethod, cancellationToken);

            return SuccessResult<PaymentMethodViewModel>.Create(PaymentMethodViewModel.FromPaymentMethod(paymentMethod));
        }
    }
}
