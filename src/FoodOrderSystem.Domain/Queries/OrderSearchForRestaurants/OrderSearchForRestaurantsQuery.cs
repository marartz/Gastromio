namespace FoodOrderSystem.Domain.Queries.OrderSearchForRestaurants
{
    public class OrderSearchForRestaurantsQuery : IQuery
    {
        public OrderSearchForRestaurantsQuery(string searchPhrase)
        {
            SearchPhrase = searchPhrase;
        }

        public string SearchPhrase { get; }
    }
}
