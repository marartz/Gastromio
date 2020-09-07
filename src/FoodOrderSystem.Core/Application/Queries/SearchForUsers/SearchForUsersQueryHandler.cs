using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Queries.SearchForUsers
{
    public class SearchForUsersQueryHandler : IQueryHandler<SearchForUsersQuery, PagingDTO<UserDTO>>
    {
        private readonly IUserRepository userRepository;

        public SearchForUsersQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<PagingDTO<UserDTO>>> HandleAsync(SearchForUsersQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return FailureResult<PagingDTO<UserDTO>>.Unauthorized();

            if (currentUser.Role < Role.RestaurantAdmin)
                return FailureResult<PagingDTO<UserDTO>>.Forbidden();

            var (total, items) = await userRepository.SearchPagedAsync(query.SearchPhrase, query.Role, query.Skip,
                query.Take, cancellationToken);

            var pagingViewModel = new PagingDTO<UserDTO>((int) total, query.Skip, query.Take,
                items.Select(user => new UserDTO(user)).ToList());

            return SuccessResult<PagingDTO<UserDTO>>.Create(pagingViewModel);
        }
    }
}
