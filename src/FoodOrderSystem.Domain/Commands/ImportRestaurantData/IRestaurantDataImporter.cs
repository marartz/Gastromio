using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.ImportRestaurantData
{
    public interface IRestaurantDataImporter
    {
        Task ImportRestaurantAsync(ImportLog log, int rowIndex, RestaurantRow restaurantRow,
            UserId curUserId, bool dryRun, CancellationToken cancellationToken = default);
    }
}