using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Domain.Model.Community
{
    public interface ICommunityFactory
    {
        Result<Community> Create(string name, UserId createdBy);
    }
}