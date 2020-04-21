namespace FoodOrderSystem.Domain.Model.User
{
    public interface IUserFactory
    {
        User Create(string name, Role role, string password);
    }
}
