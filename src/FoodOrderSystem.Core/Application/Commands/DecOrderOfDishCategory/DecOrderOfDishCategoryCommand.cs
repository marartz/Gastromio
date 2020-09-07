using FoodOrderSystem.Core.Domain.Model.DishCategory;

namespace FoodOrderSystem.Core.Application.Commands.DecOrderOfDishCategory
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