using System;

namespace Gastromio.Persistence.MongoDB.Migrations
{
    public static class DatabaseVersions
    {
        public static Version LatestVersion => MoveDishesToRestaurantSchema;

        public static Version Initial => new Version(1, 0, 0);
        public static Version CorrectRestaurantAliases => new Version(1, 0, 1);
        public static Version MoveDishesToRestaurantSchema => new Version(2, 0, 0);
    }
}
