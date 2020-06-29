﻿using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.User;
using NPOI.XSSF.UserModel;

namespace FoodOrderSystem.Domain.Commands.ImportRestaurantData
{
    public class ImportRestaurantDataCommandHandler : ICommandHandler<ImportRestaurantDataCommand, RestaurantImportLog>
    {
        private readonly IRestaurantDataImporter restaurantDataImporter;

        public ImportRestaurantDataCommandHandler(IRestaurantDataImporter restaurantDataImporter)
        {
            this.restaurantDataImporter = restaurantDataImporter;
        }

        public async Task<Result<RestaurantImportLog>> HandleAsync(ImportRestaurantDataCommand command,
            User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<RestaurantImportLog>.Unauthorized().Cast<RestaurantImportLog>();

            if (currentUser.Role < Role.RestaurantAdmin)
                return FailureResult<RestaurantImportLog>.Forbidden().Cast<RestaurantImportLog>();

            return await ProcessFileAsync(command.RestaurantDataStream, currentUser.Id, command.DryRun);
        }

        private async Task<Result<RestaurantImportLog>> ProcessFileAsync(Stream stream, UserId curUserId, bool dryRun)
        {
            var log = new RestaurantImportLog();

            var xssWorkbook = new XSSFWorkbook(stream);
            var sheet = xssWorkbook.GetSheetAt(0);
            var headerRow = sheet.GetRow(0);

            for (var rowIndex = sheet.FirstRowNum + 1; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);

                var restaurantRow = new RestaurantRow();

                try
                {
                    var cell = row.GetCell(0);
                    var timestamp = cell?.ToString();
                    if (string.IsNullOrWhiteSpace(timestamp))
                        continue;

                    cell = row.GetCell(1);
                    restaurantRow.ResponsiblePerson = cell?.StringCellValue;

                    cell = row.GetCell(2);
                    restaurantRow.AdministratorUserEmailAddress = cell?.StringCellValue;

                    cell = row.GetCell(3);
                    restaurantRow.OrderEmailAddress = cell?.StringCellValue;

                    cell = row.GetCell(4);
                    restaurantRow.Name = cell?.StringCellValue;

                    cell = row.GetCell(5);
                    restaurantRow.Phone = cell?.StringCellValue;

                    cell = row.GetCell(6);
                    restaurantRow.Street = cell?.StringCellValue;

                    cell = row.GetCell(7);
                    restaurantRow.ZipCode = cell?.NumericCellValue.ToString(CultureInfo.InvariantCulture);

                    cell = row.GetCell(8);
                    restaurantRow.City = cell?.StringCellValue;

                    cell = row.GetCell(10);
                    restaurantRow.WebSite = cell?.StringCellValue;

                    cell = row.GetCell(12);
                    restaurantRow.OrderTypes = cell?.StringCellValue;

                    cell = row.GetCell(13);
                    restaurantRow.MinimumOrderValuePickup = cell?.NumericCellValue;

                    cell = row.GetCell(14);
                    restaurantRow.MinimumOrderValueDelivery = cell?.NumericCellValue;

                    cell = row.GetCell(15);
                    restaurantRow.DeliveryCosts = cell?.NumericCellValue;

                    cell = row.GetCell(16);
                    var averageTime = cell?.DateCellValue.TimeOfDay;
                    if (averageTime.HasValue && averageTime.Value.TotalSeconds > 0)
                    {
                        restaurantRow.AverageTime = cell?.DateCellValue.TimeOfDay;
                    }
                    else
                    {
                        restaurantRow.AverageTime = null;
                    }

                    cell = row.GetCell(17);
                    restaurantRow.Cuisines = cell?.StringCellValue;

                    cell = row.GetCell(18);
                    restaurantRow.PaymentMethods = cell?.StringCellValue;

                    cell = row.GetCell(19);
                    restaurantRow.OpeningHoursMonday = cell?.StringCellValue;

                    cell = row.GetCell(20);
                    restaurantRow.OpeningHoursTuesday = cell?.StringCellValue;

                    cell = row.GetCell(21);
                    restaurantRow.OpeningHoursWednesday = cell?.StringCellValue;

                    cell = row.GetCell(22);
                    restaurantRow.OpeningHoursThursday = cell?.StringCellValue;

                    cell = row.GetCell(23);
                    restaurantRow.OpeningHoursFriday = cell?.StringCellValue;

                    cell = row.GetCell(24);
                    restaurantRow.OpeningHoursSaturday = cell?.StringCellValue;

                    cell = row.GetCell(25);
                    restaurantRow.OpeningHoursSunday = cell?.StringCellValue;

                    cell = row.GetCell(26);
                    restaurantRow.HygienicHandling = cell?.StringCellValue;

                    cell = row.GetCell(27);
                    restaurantRow.Fax = cell?.StringCellValue;

                    cell = row.GetCell(28);
                    restaurantRow.ImportId = cell?.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    log.AddLine(RestaurantImportLogLineType.Error, rowIndex + 1, "Ausnahme: {0}", e.ToString());
                }

                await restaurantDataImporter.ImportRestaurantAsync(log, rowIndex + 1, restaurantRow, curUserId,
                    dryRun);
            }

            return SuccessResult<RestaurantImportLog>.Create(log);
        }
    }
}