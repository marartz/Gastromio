using FoodOrderSystem.Domain.Model.Cuisine;

namespace FoodOrderSystem.Domain.Commands.ChangeCuisine
{
    public class ChangeCuisineCommand : ICommand
    {
        public ChangeCuisineCommand(CuisineId cuisineId, string name, byte[] image)
        {
            CuisineId = cuisineId;
            Name = name;
            Image = image;
        }

        public CuisineId CuisineId { get; }
        public string Name { get; }
        public byte[] Image { get; }
    }
}
