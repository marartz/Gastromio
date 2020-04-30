using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Queries.SearchForUsers
{
    public class SearchForUsersQuery : IQuery<ICollection<UserViewModel>>
    {
        public SearchForUsersQuery(string searchPhrase)
        {
            SearchPhrase = searchPhrase;
        }

        public string SearchPhrase { get; }
    }
}
