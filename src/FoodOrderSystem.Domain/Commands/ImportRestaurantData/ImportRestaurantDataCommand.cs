using System.IO;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.ImportRestaurantData
{
    public class ImportRestaurantDataCommand : ICommand<ImportLog>
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