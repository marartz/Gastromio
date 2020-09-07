using FoodOrderSystem.Core.Application.DTOs;

namespace FoodOrderSystem.Core.Application.Queries.SysAdminSearchForRestaurants
{
    public class SysAdminSearchForRestaurantsQuery : IQuery<PagingDTO<RestaurantDTO>>
    {
        public SysAdminSearchForRestaurantsQuery(string searchPhrase, int skip = 0, int take = -1)
        {
            SearchPhrase = searchPhrase;
            Skip = skip;
            Take = take;
        }

        public string SearchPhrase { get; }
        public int Skip { get; }
        public int Take { get; }
    }
}
