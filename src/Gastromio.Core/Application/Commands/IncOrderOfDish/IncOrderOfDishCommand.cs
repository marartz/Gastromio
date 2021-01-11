using Gastromio.Core.Domain.Model.Dish;

namespace Gastromio.Core.Application.Commands.IncOrderOfDish
{
    public class IncOrderOfDishCommand : ICommand<bool>
    {
        public IncOrderOfDishCommand(DishId dishId)
        {
            DishId = dishId;
        }
        
        public DishId DishId { get; } 
    }
}