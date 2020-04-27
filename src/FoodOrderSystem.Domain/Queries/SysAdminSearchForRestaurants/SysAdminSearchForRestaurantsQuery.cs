namespace FoodOrderSystem.Domain.Queries.SysAdminSearchForRestaurants
{
    public class SysAdminSearchForRestaurantsQuery : IQuery
    {
        public SysAdminSearchForRestaurantsQuery(string searchPhrase)
        {
            SearchPhrase = searchPhrase;
        }

        public string SearchPhrase { get; }
    }
}
