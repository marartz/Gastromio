using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Queries.SearchForUsers
{
    public class SearchForUsersQuery : IQuery<PagingViewModel<UserViewModel>>
    {
        public SearchForUsersQuery(string searchPhrase, int skip = 0, int take = -1)
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
