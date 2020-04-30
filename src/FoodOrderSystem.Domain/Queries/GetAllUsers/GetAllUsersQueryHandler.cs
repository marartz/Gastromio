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

        public async Task<QueryResult<ICollection<UserViewModel>>> HandleAsync(GetAllUsersQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return new UnauthorizedQueryResult<ICollection<UserViewModel>>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenQueryResult<ICollection<UserViewModel>>();

            var users = await userRepository.FindAllAsync(cancellationToken);

            return new SuccessQueryResult<ICollection<UserViewModel>>(users.Select(UserViewModel.FromUser).ToList());
        }
    }
}
