using System;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Model.Cuisine
{
    public interface ICuisineFactory
    {
        Result<Cuisine> Create(string name, UserId createdBy);
    }
}
