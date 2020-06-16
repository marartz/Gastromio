using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Commands.IncOrderOfDish
{
    public class IncOrderOfDishCommandHandler : ICommandHandler<IncOrderOfDishCommand, bool>
    {
        private readonly IDishRepository dishRepository;

        public IncOrderOfDishCommandHandler(IDishRepository dishRepository)
        {
            this.dishRepository = dishRepository;
        }

        public async Task<Result<bool>> HandleAsync(IncOrderOfDishCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<bool>.Unauthorized();

            if (currentUser.Role < Role.RestaurantAdmin)
                return FailureResult<bool>.Forbidden();

            var curDish =
                await dishRepository.FindByDishIdAsync(command.DishId, cancellationToken);
            if (curDish == null)
                return FailureResult<bool>.Create(FailureResultCode.DishDoesNotExist);

            var curDishes =
                (await dishRepository.FindByDishCategoryIdAsync(curDish.CategoryId, cancellationToken))
                .OrderBy(en => en.OrderNo).ToList();

            var pos = curDishes.FindIndex(en => en.Id == command.DishId);

            if (pos >= curDishes.Count - 1)
                return SuccessResult<bool>.Create(true);
            
            var tempResult = curDishes[pos].ChangeOrderNo(pos + 1, currentUser.Id);
            if (tempResult.IsFailure)
                return tempResult;

            tempResult = curDishes[pos + 1].ChangeOrderNo(pos, currentUser.Id);
            if (tempResult.IsFailure)
                return tempResult;

            await dishRepository.StoreAsync(curDishes[pos], cancellationToken);
            await dishRepository.StoreAsync(curDishes[pos + 1], cancellationToken);

            return SuccessResult<bool>.Create(true);
        }
    }
}