namespace Gastromio.Core.Domain.Model.Users
{
    public interface IUserFactory
    {
        User Create(
            Role role,
            string email,
            string password,
            bool checkPasswordPolicy,
            UserId createdBy
        );
    }
}
