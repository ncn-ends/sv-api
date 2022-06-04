using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using app.Utilities;
using FluentAssertions;
using tests.UtilityTests.Brokers;
using Xunit;

namespace tests.UtilityTests;

public class BearerToken_Tests
{
    private const string grant_type = "client_credentials";
    private readonly TokenRequestBody _reqBody;

    public BearerToken_Tests()
    {
        var credentials = Auth0ConfigBroker.GetCredentials();

        credentials.grant_type = grant_type;

        _reqBody = credentials;
    }

    [Fact]
    public async Task ShouldGetAuthToken()
    {
        var bearerToken = new BearerToken(_reqBody);
        var userToken = await bearerToken.Fetch();

        userToken.Should().BeOfType<UserToken>();

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(userToken?.access_token);
        var tokenS = jsonToken as JwtSecurityToken;

        tokenS.Should().BeOfType<JwtSecurityToken>();
    }
}