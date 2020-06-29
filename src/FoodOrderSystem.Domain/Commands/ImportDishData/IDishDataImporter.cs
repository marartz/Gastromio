using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.ImportDishData
{
    public interface IDishDataImporter
    {
        Task ImportDishAsync(ImportLog log, int rowIndex, DishRow dishRow,
            UserId curUserId, bool dryRun, CancellationToken cancellationToken = default);
    }
}