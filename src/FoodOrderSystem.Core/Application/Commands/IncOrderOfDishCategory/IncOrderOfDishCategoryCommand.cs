using FoodOrderSystem.Core.Domain.Model.DishCategory;

namespace FoodOrderSystem.Core.Application.Commands.IncOrderOfDishCategory
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