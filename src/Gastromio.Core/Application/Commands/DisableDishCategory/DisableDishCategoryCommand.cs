using Gastromio.Core.Domain.Model.DishCategory;

namespace Gastromio.Core.Application.Commands.DisableDishCategory
{
    public class DisableDishCategoryCommand : ICommand<bool>
    {
        public DisableDishCategoryCommand(DishCategoryId categoryId)
        {
            CategoryId = categoryId;
        }

        public DishCategoryId CategoryId { get; }
    }
}
