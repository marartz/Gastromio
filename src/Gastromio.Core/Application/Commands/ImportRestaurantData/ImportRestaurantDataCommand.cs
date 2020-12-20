using System.IO;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Domain.Services;

namespace Gastromio.Core.Application.Commands.ImportRestaurantData
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