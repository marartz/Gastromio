using System.Collections.Generic;
using FoodOrderSystem.Core.Application.DTOs;

namespace FoodOrderSystem.Core.Application.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IQuery<ICollection<UserDTO>>
    {
    }
}
