using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, ICollection<UserViewModel>>
    {
        private readonly IUserRepository userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<ICollection<UserViewModel>>> HandleAsync(GetAllUsersQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return FailureResult<ICollection<UserViewModel>>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<ICollection<UserViewModel>>.Forbidden();

            var users = await userRepository.FindAllAsync(cancellationToken);

            return SuccessResult<ICollection<UserViewModel>>.Create(users.Select(UserViewModel.FromUser).ToList());
        }
    }
}
