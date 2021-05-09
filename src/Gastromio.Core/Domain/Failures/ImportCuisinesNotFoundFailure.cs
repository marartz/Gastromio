using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ImportCuisinesNotFoundFailure : Failure
    {
        public override string ToString()
        {
            return "Keine Cuisine angegeben";
        }
    }
}
