using System;
using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class ExternalMenuId : ValueType<Guid>
    {
        public ExternalMenuId(Guid value) : base(value)
        {
            if (value == Guid.Empty)
                throw new InvalidOperationException("external menu id is invalid");
        }
    }
}
