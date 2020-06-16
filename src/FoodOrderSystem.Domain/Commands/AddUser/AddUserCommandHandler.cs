using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.AddUser
{
    public class AddUserCommandHandler : ICommandHandler<AddUserCommand, UserViewModel>
    {
        private readonly IUserFactory userFactory;
        private readonly IUserRepository userRepository;

        public AddUserCommandHandler(IUserFactory userFactory, IUserRepository userRepository)
        {
            this.userFactory = userFactory;
            this.userRepository = userRepository;
        }

        public async Task<Result<UserViewModel>> HandleAsync(AddUserCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<UserViewModel>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<UserViewModel>.Forbidden();

            var user = await userRepository.FindByEmailAsync(command.Email, cancellationToken);
            if (user != null)
                return FailureResult<UserViewModel>.Create(FailureResultCode.UserAlreadyExists);

            var createResult = userFactory.Create(
                command.Role,
                command.Email,
                command.Password,
                currentUser.Id
            );
            
            if (createResult.IsFailure)
                return createResult.Cast<UserViewModel>();

            user = createResult.Value;

            await userRepository.StoreAsync(user, cancellationToken);

            return SuccessResult<UserViewModel>.Create(UserViewModel.FromUser(user));
        }
    }
}
