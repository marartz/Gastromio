using FoodOrderSystem.Domain.Model.Dish;

namespace FoodOrderSystem.Domain.Commands.IncOrderOfDish
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