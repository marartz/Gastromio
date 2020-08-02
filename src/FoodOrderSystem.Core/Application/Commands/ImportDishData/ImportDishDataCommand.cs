using System.IO;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Domain.Services;

namespace FoodOrderSystem.Core.Application.Commands.ImportDishData
{
    public class ImportDishDataCommand : ICommand<ImportLog>
    {
        public ImportDishDataCommand(Stream dishDataStream, bool dryRun)
        {
            DishDataStream = dishDataStream;
            DryRun = dryRun;
        }
        
        public Stream DishDataStream { get; }
        public bool DryRun { get; }
    }
}