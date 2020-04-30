using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IQuery<ICollection<UserViewModel>>
    {
    }
}
