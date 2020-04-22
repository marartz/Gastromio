using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.ChangeCuisine
{
    public class ChangeCuisineCommandHandler : ICommandHandler<ChangeCuisineCommand>
    {
        private readonly ICuisineRepository cuisineRepository;

        public ChangeCuisineCommandHandler(ICuisineRepository cuisineRepository)
        {
            this.cuisineRepository = cuisineRepository;
        }

        public async Task<CommandResult> HandleAsync(ChangeCuisineCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult();

            var cuisine = await cuisineRepository.FindByCuisineIdAsync(command.CuisineId, cancellationToken);
            if (cuisine == null)
                return new FailureCommandResult<string>("user does not exist");

            cuisine.Change(command.Name);

            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            return new SuccessCommandResult<Cuisine>(cuisine);
        }
    }
}
