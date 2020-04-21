using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.SearchForUsers
{
    public class SearchForUsersQueryHandler : IQueryHandler<SearchForUsersQuery>
    {
        private readonly IUserRepository userRepository;

        public SearchForUsersQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<QueryResult> HandleAsync(SearchForUsersQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return new UnauthorizedQueryResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenQueryResult();

            return new SuccessQueryResult<ICollection<User>>(await userRepository.SearchAsync(query.SearchPhrase, cancellationToken));
        }
    }
}
