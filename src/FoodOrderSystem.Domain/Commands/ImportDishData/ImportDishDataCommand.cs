using System.IO;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.ImportDishData
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