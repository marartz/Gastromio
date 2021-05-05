using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Domain.Model.Cuisines
{
    public interface ICuisineFactory
    {
        Cuisine Create(string name, UserId createdBy);
    }
}
