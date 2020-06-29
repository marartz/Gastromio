using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Queries.SearchForUsers
{
    public class SearchForUsersQuery : IQuery<PagingViewModel<UserViewModel>>
    {
        public SearchForUsersQuery(string searchPhrase, Role? role, int skip = 0, int take = -1)
        {
            SearchPhrase = searchPhrase;
            Role = role;
            Skip = skip;
            Take = take;
        }

        public string SearchPhrase { get; }
        public Role? Role { get; }
        public int Skip { get; }
        public int Take { get; }
    }
}
