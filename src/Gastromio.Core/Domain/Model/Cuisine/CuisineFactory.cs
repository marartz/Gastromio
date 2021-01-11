using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Domain.Model.Cuisine
{
    public class CuisineFactory : ICuisineFactory
    {
        public Result<Cuisine> Create(string name, UserId createdBy)
        {
            var cuisine = new Cuisine(
                new CuisineId(Guid.NewGuid()),
                DateTime.UtcNow,
                createdBy,
                DateTime.UtcNow,
                createdBy
            );
            var tempResult = cuisine.ChangeName(name, createdBy);
            return tempResult.IsSuccess ? SuccessResult<Cuisine>.Create(cuisine) : tempResult.Cast<Cuisine>();
        }
    }
}