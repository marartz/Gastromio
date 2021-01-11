using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Domain.Model.Cuisine
{
    public interface ICuisineFactory
    {
        Result<Cuisine> Create(string name, UserId createdBy);
        
        Result<Cuisine> Create(string name, string image, UserId createdBy);
    }
}
