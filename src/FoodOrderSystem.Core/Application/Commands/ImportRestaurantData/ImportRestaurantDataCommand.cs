using System.IO;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Domain.Services;

namespace FoodOrderSystem.Core.Application.Commands.ImportRestaurantData
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