using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Domain.Services
{
    public interface IRestaurantDataImporter
    {
        Task ImportRestaurantAsync(ImportLog log, int rowIndex, RestaurantRow restaurantRow,
            UserId curUserId, bool dryRun, CancellationToken cancellationToken = default);
    }
}