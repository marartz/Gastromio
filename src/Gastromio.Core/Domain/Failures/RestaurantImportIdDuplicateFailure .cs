using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantImportIdDuplicateFailure : Failure
    {
        public override string ToString()
        {
            return "Import-Id existiert bereits!";
        }
    }
}
