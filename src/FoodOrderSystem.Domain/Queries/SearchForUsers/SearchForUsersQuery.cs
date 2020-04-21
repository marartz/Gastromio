namespace FoodOrderSystem.Domain.Queries.SearchForUsers
{
    public class SearchForUsersQuery : IQuery
    {
        public SearchForUsersQuery(string searchPhrase)
        {
            SearchPhrase = searchPhrase;
        }

        public string SearchPhrase { get; }
    }
}
