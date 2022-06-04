using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using app;
using app.Models;
using app.Utilities;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using tests.UtilityTests.Brokers;
using Xunit;
using Newtonsoft.Json;

namespace tests.UtilityTests;

public class TestUserBroker_Tests
{
    [Fact]
    public async Task ShouldGetUserAuthToken()
    {
        var broker = new TestUserBroker();
        await broker.InitializeToken();
        
        broker.DecodedToken.Should().BeOfType<JwtSecurityToken>();
        var userInfoClaim = broker.DecodedToken.Claims.FirstOrDefault(c => c.Type == "https://sv.com/info");
        var authUser = JsonConvert.DeserializeObject<TokenUser>(userInfoClaim.Value);
        
        authUser.email.Should().NotBe(null);
        authUser.profile_id.Should().NotBe(null);
    }
}