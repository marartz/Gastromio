using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.User;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.GetCategoriesOfRestaurant
{
    public class GetCategoriesOfRestaurantQueryHandler : IQueryHandler<GetCategoriesOfRestaurantQuery>
    {
        private readonly IDishCategoryRepository categoryRepository;

        public GetCategoriesOfRestaurantQueryHandler(IDishCategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<QueryResult> HandleAsync(GetCategoriesOfRestaurantQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            return new SuccessQueryResult<ICollection<DishCategory>>(await categoryRepository.FindByRestaurantIdAsync(query.RestaurantId, cancellationToken));
        }
    }
}
