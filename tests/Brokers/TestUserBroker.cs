using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Threading.Tasks;
using app.Utilities;
using FluentAssertions;

namespace tests.UtilityTests.Brokers;

public class TestUserBroker
{
    private const string grant_type = "password";
    private const string scope = "profile email";
    private const string username = "testuser@test.com";
    private const string password = "Test.Password.123";
    private readonly TokenRequestBody _reqBody;
    public string BearerToken;
    public JwtSecurityToken DecodedToken;

    public TestUserBroker()
    {
        var credentials = Auth0ConfigBroker.GetCredentials();

        credentials.username = username;
        credentials.password = password;
        credentials.scope = scope;
        credentials.grant_type = grant_type;

        _reqBody = credentials;
    }

    public async Task InitializeToken()
    {
        var bearerToken = new BearerToken(_reqBody);
        var userToken = await bearerToken.Fetch();

        BearerToken = userToken?.access_token 
                      ?? throw new AuthenticationException();

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(userToken.access_token);
        
        DecodedToken = jsonToken as JwtSecurityToken 
                       ?? throw new AuthenticationException();
    }
}