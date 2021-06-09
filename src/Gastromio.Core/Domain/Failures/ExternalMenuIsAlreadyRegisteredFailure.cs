using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Domain.Failures
{
    public class ExternalMenuIsAlreadyRegisteredFailure : Failure
    {
        private readonly ExternalMenuId externalMenuId;

        public ExternalMenuIsAlreadyRegisteredFailure(ExternalMenuId externalMenuId)
        {
            this.externalMenuId = externalMenuId;
        }

        public override string ToString()
        {
            return $"Externe Speisekarte mit Id {externalMenuId} ist bereits registriert";
        }
    }
}
