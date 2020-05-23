namespace FoodOrderSystem.Domain.Model.User
{
    public interface IUserFactory
    {
        Result<User> Create(string name, Role role, string email, string password);
    }
}
