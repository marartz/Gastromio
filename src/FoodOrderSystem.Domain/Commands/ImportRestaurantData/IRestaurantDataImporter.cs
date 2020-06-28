using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Commands.ImportRestaurantData
{
    public interface IRestaurantDataImporter
    {
        Task ImportRestaurantAsync(RestaurantImportLog log, int rowIndex, RestaurantRow restaurantRow,
            UserId curUserId, bool dryRun, CancellationToken cancellationToken = default);
    }
}