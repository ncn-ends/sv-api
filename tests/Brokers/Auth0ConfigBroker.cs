using System.IO;
using app.Utilities;
using Microsoft.Extensions.Configuration;

namespace tests.UtilityTests.Brokers;

public static class Auth0ConfigBroker
{
    public static TokenRequestBody GetCredentials()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<UserCreds_Tests>()
            .Build();

        var credentials = configuration.GetSection("Auth0").Get<TokenRequestBody>();

        return credentials;
    }
}