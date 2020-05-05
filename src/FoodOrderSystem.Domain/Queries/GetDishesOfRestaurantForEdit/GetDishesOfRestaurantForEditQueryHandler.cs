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

namespace FoodOrderSystem.Domain.Queries.GetDishesOfRestaurantForEdit
{
    public class GetDishesOfRestaurantForEditQueryHandler : IQueryHandler<GetDishesOfRestaurantForEditQuery, ICollection<DishCategoryViewModel>>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IDishCategoryRepository dishCategoryRepository;
        private readonly IDishRepository dishRepository;

        public GetDishesOfRestaurantForEditQueryHandler(
            IRestaurantRepository restaurantRepository,
            IDishCategoryRepository dishCategoryRepository,
            IDishRepository dishRepository
        )
        {
            this.restaurantRepository = restaurantRepository;
            this.dishCategoryRepository = dishCategoryRepository;
            this.dishRepository = dishRepository;
        }

        public async Task<Result<ICollection<DishCategoryViewModel>>> HandleAsync(GetDishesOfRestaurantForEditQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return FailureResult<ICollection<DishCategoryViewModel>>.Unauthorized();

            if (currentUser.Role < Role.RestaurantAdmin)
                return FailureResult<ICollection<DishCategoryViewModel>>.Forbidden();

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(query.RestaurantId, cancellationToken);
            if (restaurant == null)
                return FailureResult<ICollection<DishCategoryViewModel>>.Create(FailureResultCode.RestaurantDoesNotExist);

            var dishCategories = await dishCategoryRepository.FindByRestaurantIdAsync(query.RestaurantId, cancellationToken);
            var dishes = await dishRepository.FindByRestaurantIdAsync(query.RestaurantId, cancellationToken);

            var result = new List<DishCategoryViewModel>();

            if (dishCategories != null)
            {
                foreach (var dishCategory in dishCategories)
                {
                    var dishViewModels = new List<DishViewModel>();

                    if (dishes != null)
                    {
                        foreach (var dish in dishes.Where(en => en.CategoryId == dishCategory.Id))
                        {
                            var variantViewModels = new List<DishVariantViewModel>();

                            if (dish.Variants != null)
                            {
                                foreach (var variant in dish.Variants)
                                {
                                    var extraViewModels = new List<DishVariantExtraViewModel>();

                                    if (variant.Extras != null)
                                    {
                                        foreach (var extra in variant.Extras)
                                        {
                                            extraViewModels.Add(new DishVariantExtraViewModel(extra.Name, extra.ProductInfo, extra.Price));
                                        }
                                    }

                                    variantViewModels.Add(new DishVariantViewModel(variant.Name, variant.Price, extraViewModels));
                                }
                            }

                            dishViewModels.Add(new DishViewModel(dish.Id.Value, dish.Name, dish.Description, dish.ProductInfo, variantViewModels));
                        }
                    }

                    result.Add(new DishCategoryViewModel(dishCategory.Id.Value, dishCategory.Name, dishViewModels));
                }
            }

            return SuccessResult<ICollection<DishCategoryViewModel>>.Create(result);
        }
    }
}
