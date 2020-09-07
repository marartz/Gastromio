using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Queries.SearchForUsers
{
    public class SearchForUsersQuery : IQuery<PagingDTO<UserDTO>>
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
