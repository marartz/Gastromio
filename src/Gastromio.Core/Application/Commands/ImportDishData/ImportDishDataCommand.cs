using System.IO;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Domain.Services;

namespace Gastromio.Core.Application.Commands.ImportDishData
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