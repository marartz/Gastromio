using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.GetDishesOfRestaurant
{
    public class GetDishesOfRestaurantQueryHandler : IQueryHandler<GetDishesOfRestaurantQuery, ICollection<DishCategoryViewModel>>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IDishCategoryRepository dishCategoryRepository;
        private readonly IDishRepository dishRepository;

        public GetDishesOfRestaurantQueryHandler(
            IRestaurantRepository restaurantRepository,
            IDishCategoryRepository dishCategoryRepository,
            IDishRepository dishRepository
        )
        {
            this.restaurantRepository = restaurantRepository;
            this.dishCategoryRepository = dishCategoryRepository;
            this.dishRepository = dishRepository;
        }

        public async Task<Result<ICollection<DishCategoryViewModel>>> HandleAsync(GetDishesOfRestaurantQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(query.RestaurantId, cancellationToken);
            if (restaurant == null)
                return FailureResult<ICollection<DishCategoryViewModel>>.Create(FailureResultCode.RestaurantDoesNotExist);

            var dishCategories = await dishCategoryRepository.FindByRestaurantIdAsync(query.RestaurantId, cancellationToken);
            var dishes = await dishRepository.FindByRestaurantIdAsync(query.RestaurantId, cancellationToken);

            var result = new List<DishCategoryViewModel>();

            if (dishCategories != null)
            {
                foreach (var dishCategory in dishCategories.OrderBy(en => en.OrderNo))
                {
                    var dishViewModels = new List<DishViewModel>();

                    if (dishes != null)
                    {
                        foreach (var dish in dishes.Where(en => en.CategoryId == dishCategory.Id).OrderBy(en => en.OrderNo))
                        {
                            var variantViewModels = new List<DishVariantViewModel>();

                            if (dish.Variants != null)
                            {
                                variantViewModels.AddRange(dish.Variants.Select(variant => new DishVariantViewModel
                                {
                                    VariantId = variant.VariantId, Name = variant.Name, Price = variant.Price,
                                    Extras = new List<DishVariantExtraViewModel>()
                                }));
                            }

                            dishViewModels.Add(new DishViewModel
                            {
                                Id = dish.Id.Value,
                                Name = dish.Name,
                                Description = dish.Description,
                                ProductInfo = dish.ProductInfo,
                                Variants = variantViewModels
                            });
                        }
                    }

                    result.Add(new DishCategoryViewModel
                    {
                        Id = dishCategory.Id.Value,
                        Name = dishCategory.Name,
                        Dishes = dishViewModels
                    });
                }
            }

            return SuccessResult<ICollection<DishCategoryViewModel>>.Create(result);
        }
    }
}
