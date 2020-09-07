using FoodOrderSystem.Core.Domain.Model.Dish;

namespace FoodOrderSystem.Core.Application.Commands.IncOrderOfDish
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