using FoodOrderSystem.Domain.Model.Dish;

namespace FoodOrderSystem.Domain.Commands.DecOrderOfDish
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