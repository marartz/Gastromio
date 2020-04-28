using System;

namespace FoodOrderSystem.Domain.Model.Cuisine
{
    public class CuisineFactory : ICuisineFactory
    {
        public Cuisine Create(string name, byte[] image)
        {
            return new Cuisine(new CuisineId(Guid.NewGuid()), name, image);
        }
    }
}
