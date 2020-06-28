using System.IO;

namespace FoodOrderSystem.Domain.Commands.ImportRestaurantData
{
    public class ImportRestaurantDataCommand : ICommand<RestaurantImportLog>
    {
        public ImportRestaurantDataCommand(Stream restaurantDataStream, bool dryRun)
        {
            RestaurantDataStream = restaurantDataStream;
            DryRun = dryRun;
        }
        
        public Stream RestaurantDataStream { get; }
        public bool DryRun { get; }
    }
}