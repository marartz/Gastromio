using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.SearchForUsers
{
    public class SearchForUsersQueryHandler : IQueryHandler<SearchForUsersQuery, PagingViewModel<UserViewModel>>
    {
        private readonly IUserRepository userRepository;

        public SearchForUsersQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<PagingViewModel<UserViewModel>>> HandleAsync(SearchForUsersQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return FailureResult<PagingViewModel<UserViewModel>>.Unauthorized();

            if (currentUser.Role < Role.RestaurantAdmin)
                return FailureResult<PagingViewModel<UserViewModel>>.Forbidden();

            var (total, items) = await userRepository.SearchPagedAsync(query.SearchPhrase, query.Role, query.Skip,
                query.Take, cancellationToken);

            var pagingViewModel = new PagingViewModel<UserViewModel>((int) total, query.Skip, query.Take,
                items.Select(UserViewModel.FromUser).ToList());

            return SuccessResult<PagingViewModel<UserViewModel>>.Create(pagingViewModel);
        }
    }
}
