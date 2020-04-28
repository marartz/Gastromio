using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.AddCuisine
{
    public class AddCuisineCommandHandler : ICommandHandler<AddCuisineCommand>
    {
        private readonly ICuisineFactory cuisineFactory;
        private readonly ICuisineRepository cuisineRepository;

        public AddCuisineCommandHandler(ICuisineFactory cuisineFactory, ICuisineRepository cuisineRepository)
        {
            this.cuisineFactory = cuisineFactory;
            this.cuisineRepository = cuisineRepository;
        }

        public async Task<CommandResult> HandleAsync(AddCuisineCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult();

            var cuisine = await cuisineRepository.FindByNameAsync(command.Name, cancellationToken);
            if (cuisine != null)
                return new FailureCommandResult<string>("cuisine name already exists");

            cuisine = cuisineFactory.Create(command.Name, command.Image);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            return new SuccessCommandResult<Cuisine>(cuisine);
        }
    }
}
