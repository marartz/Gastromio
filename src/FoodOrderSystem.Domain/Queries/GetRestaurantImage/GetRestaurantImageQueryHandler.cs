using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.RestaurantImage;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Queries.GetRestaurantImage
{
    public class GetRestaurantImageQueryHandler : IQueryHandler<GetRestaurantImageQuery, byte[]>
    {
        private readonly IRestaurantImageRepository restaurantImageRepository;

        public GetRestaurantImageQueryHandler(IRestaurantImageRepository restaurantImageRepository)
        {
            this.restaurantImageRepository = restaurantImageRepository;
        }
        
        public Task<Result<byte[]>> HandleAsync(GetRestaurantImageQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}