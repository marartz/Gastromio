using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.AddCuisine
{
    public class AddCuisineCommandHandler : ICommandHandler<AddCuisineCommand, CuisineViewModel>
    {
        private readonly ICuisineFactory cuisineFactory;
        private readonly ICuisineRepository cuisineRepository;

        public AddCuisineCommandHandler(ICuisineFactory cuisineFactory, ICuisineRepository cuisineRepository)
        {
            this.cuisineFactory = cuisineFactory;
            this.cuisineRepository = cuisineRepository;
        }

        public async Task<CommandResult<CuisineViewModel>> HandleAsync(AddCuisineCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult<CuisineViewModel>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult<CuisineViewModel>();

            var cuisine = await cuisineRepository.FindByNameAsync(command.Name, cancellationToken);
            if (cuisine != null)
                return new FailureCommandResult<CuisineViewModel>();

            cuisine = cuisineFactory.Create(command.Name);
            await cuisineRepository.StoreAsync(cuisine, cancellationToken);

            return new SuccessCommandResult<CuisineViewModel>(CuisineViewModel.FromCuisine(cuisine));
        }
    }
}
