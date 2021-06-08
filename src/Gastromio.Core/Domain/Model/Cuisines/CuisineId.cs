using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Cuisines
{
    public class CuisineId : ValueType<Guid>
    {
        public CuisineId(Guid value) : base(value)
        {
            if (value == Guid.Empty)
                throw DomainException.CreateFrom(new CuisineIdIsInvalidFailure());
        }
    }
}
