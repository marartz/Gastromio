using System;

namespace FoodOrderSystem.Domain.Model.Cuisine
{
    public class Cuisine
    {
        public Cuisine(CuisineId id)
        {
            Id = id;
        }

        public Cuisine(CuisineId id, string name) : this(id)
        {
            Name = name;
        }
        
        public CuisineId Id { get; }
        public string Name { get; private set; }

        public Result<bool> ChangeName(string name)
        {
            Name = name;
            return SuccessResult<bool>.Create(true);
        }
    }
}
