using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Threading.Tasks;
using app;
using app.Utilities;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using tests.UtilityTests.Brokers;
using Xunit;

namespace tests.UtilityTests;

public class UserCreds_Tests
{
    private const string grant_type = "password";
    private const string scope = "profile email";
    private const string username = "testuser@test.com";
    private const string password = "Test.Password.123";
    private readonly TokenRequestBody _reqBody;

    public UserCreds_Tests()
    {
        var credentials = Auth0ConfigBroker.GetCredentials();
        
        credentials.username = username;
        credentials.password = password;
        credentials.scope = scope;
        credentials.grant_type = grant_type;

        _reqBody = credentials;
    }

    [Fact]
    public async Task ShouldGetUserAuthToken()
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