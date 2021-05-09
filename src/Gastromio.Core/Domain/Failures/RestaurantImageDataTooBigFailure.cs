using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantImageDataTooBigFailure : Failure
    {
        public override string ToString()
        {
            return "Die Bilddatei ist zu groß (max. 4MB)";
        }
    }
}
