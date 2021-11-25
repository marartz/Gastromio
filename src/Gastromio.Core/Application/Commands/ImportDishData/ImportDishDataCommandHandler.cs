using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Core.Domain.Services;
using NPOI.XSSF.UserModel;

namespace Gastromio.Core.Application.Commands.ImportDishData
{
    public class ImportRestaurantDataCommandHandler : ICommandHandler<ImportDishDataCommand, ImportLog>
    {
        private readonly IDishDataImporter dishDataImporter;

        public ImportRestaurantDataCommandHandler(IDishDataImporter dishDataImporter)
        {
            this.dishDataImporter = dishDataImporter;
        }

        public async Task<ImportLog> HandleAsync(ImportDishDataCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.RestaurantAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            return await ProcessFileAsync(command.DishDataStream, currentUser.Id, command.DryRun);
        }

        private async Task<ImportLog> ProcessFileAsync(Stream stream, UserId curUserId, bool dryRun)
        {
            var log = new ImportLog();

            var xssWorkbook = new XSSFWorkbook(stream);
            var sheet = xssWorkbook.GetSheetAt(0);

            for (var rowIndex = sheet.FirstRowNum + 1; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null)
                    continue;

                try
                {
                    var dishRow = new DishRow();

                    var cell = row.GetCell(13);
                    if (string.IsNullOrWhiteSpace(cell?.ToString()))
                        continue;
                    dishRow.RestaurantImportId = cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);

                    cell = row.GetCell(2);
                    dishRow.Category = cell?.StringCellValue.Trim();

                    cell = row.GetCell(3);
                    dishRow.DishName = cell?.StringCellValue.Trim();

                    cell = row.GetCell((4));
                    dishRow.Description = cell?.StringCellValue.Trim();

                    cell = row.GetCell(5);
                    dishRow.ProductInfo = cell?.StringCellValue.Trim();

                    cell = row.GetCell(6);
                    dishRow.VariantName = cell?.StringCellValue.Trim();

                    cell = row.GetCell(7);
                    dishRow.VariantPrice = cell?.NumericCellValue;

                    await dishDataImporter.ImportDishAsync(log, rowIndex + 1, dishRow, curUserId,
                        dryRun);
                }
                catch (Exception e)
                {
                    log.AddLine(ImportLogLineType.Error, rowIndex + 1, "Ausnahme: {0}", e.ToString());
                }
            }

            return log;
        }
    }
}
