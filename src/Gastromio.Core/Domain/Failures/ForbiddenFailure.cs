using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ForbiddenFailure : Failure
    {
        public override string ToString()
        {
            return "Sie sind nicht berechtigt, diese Aktion auszuführen";
        }
    }
}
