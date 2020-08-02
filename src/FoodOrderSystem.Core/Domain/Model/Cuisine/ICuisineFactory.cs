using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Domain.Model.Cuisine
{
    public interface ICuisineFactory
    {
        Result<Cuisine> Create(string name, UserId createdBy);
    }
}
