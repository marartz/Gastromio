using FoodOrderSystem.Core.Domain.Model.Dish;

namespace FoodOrderSystem.Core.Application.Commands.DecOrderOfDish
{
    public class DecOrderOfDishCommand : ICommand<bool>
    {
        public DecOrderOfDishCommand(DishId dishId)
        {
            DishId = dishId;
        }
        
        public DishId DishId { get; } 
    }
}