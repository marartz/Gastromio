using FoodOrderSystem.Domain.Model.Cuisine;

namespace FoodOrderSystem.Domain.Commands.ChangeCuisine
{
    public class ChangeCuisineCommand : ICommand<bool>
    {
        public ChangeCuisineCommand(CuisineId cuisineId, string name)
        {
            CuisineId = cuisineId;
            Name = name;
        }

        public CuisineId CuisineId { get; }
        public string Name { get; }
    }
}
