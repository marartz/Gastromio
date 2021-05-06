using Gastromio.Domain.TestKit.Common;
using Xunit;
using FluentAssertions;
using Gastromio.Persistence.MongoDB.Migrations;

namespace Gastromio.Persistence.MongoDB.Tests
{
    public class DatabaseMigrationsTest : DatabaseTestBase
    {
        [Fact]
        [Trait("Category", TestCategoryConstants.IntegrationTest)]
        public void AllValid_RunInitialSetup_ShouldBeSuccessful()
        {
            CurrentVersion.Should().Be(new MongoDBMigrations.Version(1, 0, 0));
        }

        [Fact]
        [Trait("Category", TestCategoryConstants.IntegrationTest)]
        public void AllValid_RunDatabaseMigrations_ShouldBeSuccessful()
        {
            var result = Migration.Run();
            result.CurrentVersion.Should().Be(DatabaseVersions.LatestVersion);
            IsDatabaseOutdated.Should().BeFalse();
        }
    }
}
