using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Queries.SearchForUsers
{
    public class SearchForUsersQueryHandler : IQueryHandler<SearchForUsersQuery, PagingDTO<UserDTO>>
    {
        private readonly IUserRepository userRepository;

        public SearchForUsersQueryHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<PagingDTO<UserDTO>> HandleAsync(SearchForUsersQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var (total, items) = await userRepository.SearchPagedAsync(query.SearchPhrase, query.Role, query.Skip,
                query.Take, cancellationToken);

            var pagingViewModel = new PagingDTO<UserDTO>((int) total, query.Skip, query.Take,
                items.Select(user => new UserDTO(user)).ToList());

            return pagingViewModel;
        }
    }
}
