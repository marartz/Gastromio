using FoodOrderSystem.Domain.Model.DishCategory;

namespace FoodOrderSystem.Domain.Commands.IncOrderOfDishCategory
{
    public class IncOrderOfDishCategoryCommand : ICommand<bool>
    {
        public IncOrderOfDishCategoryCommand(DishCategoryId categoryId)
        {
            CategoryId = categoryId;
        }
        
        public DishCategoryId CategoryId { get; } 
    }
}