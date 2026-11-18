using Xunit;

namespace DontaionHub.IntegrationTests
{
    [CollectionDefinition("Api")]
    public class ApiCollection : ICollectionFixture<KestrelWebApplicationFactory>
    {
    }
}