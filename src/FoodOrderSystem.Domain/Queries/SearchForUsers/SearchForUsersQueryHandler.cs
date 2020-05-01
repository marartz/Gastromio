using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.SearchForUsers
{
    public class SearchForUsersQueryHandler : IQueryHandler<SearchForUsersQuery, ICollection<UserViewModel>>
    {
        private readonly IUserRepository userRepository;

        public SearchForUsersQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<QueryResult<ICollection<UserViewModel>>> HandleAsync(SearchForUsersQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return new UnauthorizedQueryResult<ICollection<UserViewModel>>();

            if (currentUser.Role < Role.RestaurantAdmin)
                return new ForbiddenQueryResult<ICollection<UserViewModel>>();

            var users = await userRepository.SearchAsync(query.SearchPhrase, cancellationToken);

            return new SuccessQueryResult<ICollection<UserViewModel>>(users.Select(UserViewModel.FromUser).ToList());
        }
    }
}
