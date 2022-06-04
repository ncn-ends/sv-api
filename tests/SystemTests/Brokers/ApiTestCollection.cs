using Xunit;

namespace tests.SystemTests.Brokers;

[CollectionDefinition(nameof(ApiTestCollection))]
public class ApiTestCollection : ICollectionFixture<ApiHttpClientBroker>
{
}