using FoodOrderSystem.Core.Domain.Model.Cuisine;

namespace FoodOrderSystem.Core.Application.Commands.RemoveCuisine
{
    public class RemoveCuisineCommand : ICommand<bool>
    {
        public RemoveCuisineCommand(CuisineId cuisineId)
        {
            CuisineId = cuisineId;
        }

        public CuisineId CuisineId { get; }
    }
}
