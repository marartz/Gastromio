using System;

namespace FoodOrderSystem.Domain.Model.Cuisine
{
    public class CuisineFactory : ICuisineFactory
    {
        public Result<Cuisine> Create(string name)
        {
            var cuisine = new Cuisine(new CuisineId(Guid.NewGuid()));
            var tempResult = cuisine.ChangeName(name);
            return tempResult.IsSuccess ? SuccessResult<Cuisine>.Create(cuisine) : tempResult.Cast<Cuisine>();
        }
    }
}
