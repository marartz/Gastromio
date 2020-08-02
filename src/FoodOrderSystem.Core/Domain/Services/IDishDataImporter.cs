using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Domain.Services
{
    public interface IDishDataImporter
    {
        Task ImportDishAsync(ImportLog log, int rowIndex, DishRow dishRow,
            UserId curUserId, bool dryRun, CancellationToken cancellationToken = default);
    }
}