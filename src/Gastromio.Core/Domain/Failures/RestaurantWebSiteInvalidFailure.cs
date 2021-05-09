using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantWebSiteInvalidFailure : Failure
    {
        private readonly string webSite;

        public RestaurantWebSiteInvalidFailure(string webSite)
        {
            this.webSite = webSite;
        }

        public override string ToString()
        {
            return $"Die Webseite des Restaurants ist nicht gültig: {webSite}";
        }
    }
}
