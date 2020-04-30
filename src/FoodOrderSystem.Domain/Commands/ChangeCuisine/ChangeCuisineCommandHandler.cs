using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.ChangeCuisine
{
    public class ChangeCuisineCommandHandler : ICommandHandler<ChangeCuisineCommand, bool>
    {
        private readonly ICuisineRepository cuisineRepository;

        public ChangeCuisineCommandHandler(ICuisineRepository cuisineRepository)
        {
            this.cuisineRepository = cuisineRepository;
        }

        public async Task<CommandResult<bool>> HandleAsync(ChangeCuisineCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult<bool>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult<bool>();

            var cuisine = await cuisineRepository.FindByCuisineIdAsync(command.CuisineId, cancellationToken);
            if (cuisine == null)
                return new FailureCommandResult<bool>();

            cuisine.Change(command.Name);

            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            return new SuccessCommandResult<bool>(true);
        }
    }
}
