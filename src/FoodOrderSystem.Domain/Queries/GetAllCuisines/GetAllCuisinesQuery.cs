using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Queries.GetAllCuisines
{
    public class GetAllCuisinesQuery : IQuery<ICollection<CuisineViewModel>>
    {
    }
}
