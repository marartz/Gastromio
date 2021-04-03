using Gastromio.Core.Domain.Model.Cuisines;

namespace Gastromio.Core.Application.Commands.ChangeCuisine
{
    public class ChangeCuisineCommand : ICommand<bool>
    {
        public ChangeCuisineCommand(CuisineId cuisineId, string name, string image)
        {
            CuisineId = cuisineId;
            Name = name;
            Image = image;
        }

        public CuisineId CuisineId { get; }
        
        public string Name { get; }
        
        public string Image { get; }
    }
}
