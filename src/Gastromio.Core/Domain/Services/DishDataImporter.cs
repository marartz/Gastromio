using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Domain.Services
{
    public class DishDataImporter : IDishDataImporter
    {
        private readonly IRestaurantRepository restaurantRepository;

        private readonly Dictionary<string, Restaurant> recentRestaurants = new Dictionary<string, Restaurant>();

        private readonly Dictionary<string, Dictionary<string, DishCategory>> recentDishCategories =
            new Dictionary<string, Dictionary<string, DishCategory>>();

        private readonly Dictionary<string, Dictionary<string, Dish>> recentDishes =
            new Dictionary<string, Dictionary<string, Dish>>();

        public DishDataImporter(
            IRestaurantRepository restaurantRepository
        )
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task ImportDishAsync(ImportLog log, int rowIndex, DishRow dishRow,
            UserId curUserId, bool dryRun, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!recentRestaurants.TryGetValue(dishRow.RestaurantImportId, out var restaurant))
                {
                    restaurant =
                        await restaurantRepository.FindByImportIdAsync(dishRow.RestaurantImportId, cancellationToken);
                    if (restaurant == null)
                    {
                        log.AddLine(ImportLogLineType.Warning, rowIndex,
                            "Restaurant mit Import-Id '{0}' nicht gefunden => Überspringe Datensatz",
                            dishRow.RestaurantImportId);
                        return;
                    }

                    recentRestaurants.Add(dishRow.RestaurantImportId, restaurant);

                    log.AddLine(ImportLogLineType.Information, rowIndex,
                        "Lösche alle Speisen von Restaurant '{0}', wenn vorhanden", restaurant.Name);
                    restaurant.RemoveAllDishCategoriesDueToImport(curUserId);
                }

                if (!recentDishCategories.TryGetValue(dishRow.RestaurantImportId, out var dishCategoryDict))
                {
                    dishCategoryDict = new Dictionary<string, DishCategory>();
                    recentDishCategories.Add(dishRow.RestaurantImportId, dishCategoryDict);
                }

                if (!dishCategoryDict.TryGetValue(dishRow.Category, out var category))
                {
                    var lastDishCategory = dishCategoryDict.Count == 0
                        ? null
                        : dishCategoryDict
                            .Select(en => en.Value)
                            .Aggregate((i1, i2) => i1.OrderNo > i2.OrderNo ? i1 : i2);

                    category = restaurant.AddDishCategory(dishRow.Category, lastDishCategory?.Id, curUserId);
                    restaurant.EnableDishCategory(category.Id, curUserId);
                    restaurant.DishCategories.TryGetDishCategory(category.Id, out category);

                    log.AddLine(ImportLogLineType.Information, rowIndex,
                        "Lege für Restaurant '{0}' eine neue Kategorie mit Namen '{1}' und Sortierung '{2}' an",
                        restaurant.Name, dishRow.Category, category.OrderNo);

                    dishCategoryDict.Add(dishRow.Category, category);
                }

                if (!recentDishes.TryGetValue(dishRow.RestaurantImportId, out var dishDict))
                {
                    dishDict = new Dictionary<string, Dish>();
                    recentDishes.Add(dishRow.RestaurantImportId, dishDict);
                }

                var dishKey = $"{dishRow.Category}||{dishRow.DishName}||{dishRow.Description ?? ""}";

                if (!dishDict.TryGetValue(dishKey, out var dish))
                {
                    var orderNo = dishDict.Count == 0 ? 0 : dishDict.Max(en => en.Value.OrderNo) + 1;

                    var variant = new DishVariant(
                        new DishVariantId(Guid.NewGuid()),
                        string.IsNullOrWhiteSpace(dishRow.VariantName) ? string.Empty : dishRow.VariantName,
                        dishRow.VariantPrice.HasValue ? (decimal) dishRow.VariantPrice.Value : 0
                    );

                    dish = restaurant.AddOrChangeDish(category.Id, null, dishRow.DishName, dishRow.Description,
                        dishRow.ProductInfo, orderNo, new[] {variant}, curUserId);

                    log.AddLine(ImportLogLineType.Information, rowIndex,
                        "Lege für Restaurant '{0}' eine neue Speise mit Namen '{1}', Beschreibung '{2}' und Sortierung '{3}' an",
                        restaurant.Name, dishRow.DishName, dishRow.Description, orderNo);

                    dishDict.Add(dishKey, dish);
                }
                else
                {
                    var dishVariant = new DishVariant(
                        new DishVariantId(Guid.NewGuid()),
                        dishRow.VariantName,
                        dishRow.VariantPrice.HasValue ? (decimal) dishRow.VariantPrice.Value : 0
                    );

                    restaurant.AddDishVariant(category.Id, dish.Id, dishVariant, curUserId);

                    log.AddLine(ImportLogLineType.Information, rowIndex,
                        "Lege für Restaurant '{0}' und Speise '{1}' eine neue Variante '{2}' an",
                        restaurant.Name, dishRow.DishName, dishRow.VariantName);
                }

                if (!dryRun)
                {
                    await restaurantRepository.StoreAsync(restaurant, cancellationToken);
                }
            }
            catch (DomainException e)
            {
                AddFailureMessageToLog(log, rowIndex, e.Failure);
            }
        }

        private static void AddFailureMessageToLog(ImportLog log, int rowIndex, Failure failure)
        {
            log.AddLine(ImportLogLineType.Error, rowIndex, failure.Message);
        }
    }
}
