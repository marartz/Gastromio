using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Domain.Model.Community;

namespace Gastromio.Core.Application.Ports.Persistence
{
    public interface ICommunityRepository
    {
        Task<IEnumerable<Community>> FindAllAsync(CancellationToken cancellationToken = default);

        Task<Community> FindByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<Community> FindByCommunityIdAsync(CommunityId communityId, CancellationToken cancellationToken = default);

        Task StoreAsync(Community community, CancellationToken cancellationToken = default);

        Task RemoveAsync(CommunityId communityId, CancellationToken cancellationToken = default);
    }
}