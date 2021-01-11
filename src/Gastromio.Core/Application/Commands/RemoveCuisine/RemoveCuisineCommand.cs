using Gastromio.Core.Domain.Model.Cuisine;

namespace Gastromio.Core.Application.Commands.RemoveCuisine
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
