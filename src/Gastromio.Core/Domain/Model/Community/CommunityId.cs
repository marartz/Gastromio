using System;
using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Model.Community
{
    public class CommunityId : ValueType<Guid>
    {
        public CommunityId(Guid value) : base(value)
        {
        }
    }
}