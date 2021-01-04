using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Domain.Model.Community
{
    public class CommunityFactory : ICommunityFactory
    {
        public Result<Community> Create(string name, UserId createdBy)
        {
            return SuccessResult<Community>.Create(null);
        }
    }
}