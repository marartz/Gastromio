namespace FoodOrderSystem.Domain.Model.User
{
    public interface IUserFactory
    {
        Result<User> Create(Role role, string email, string password);
    }
}
