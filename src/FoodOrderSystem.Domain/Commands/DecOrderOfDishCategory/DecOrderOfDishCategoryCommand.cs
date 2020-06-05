using FoodOrderSystem.Domain.Model.DishCategory;

namespace FoodOrderSystem.Domain.Commands.DecOrderOfDishCategory
{
    public class DecOrderOfDishCategoryCommand : ICommand<bool>
    {
        public DecOrderOfDishCategoryCommand(DishCategoryId categoryId)
        {
            CategoryId = categoryId;
        }
        
        public DishCategoryId CategoryId { get; } 
    }
}