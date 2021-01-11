using System;
using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Model.Cuisine
{
    public class CuisineId : ValueType<Guid>
    {
        public CuisineId(Guid value) : base(value)
        {
        }
    }
}
