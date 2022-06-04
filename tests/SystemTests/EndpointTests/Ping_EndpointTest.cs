using System.Threading.Tasks;
using app.Models;
using FluentAssertions;
using tests.SystemTests.Brokers;
using Xunit;

namespace tests.SystemTests.EndpointTests;

[Collection(nameof(ApiTestCollection))]
public class Ping_EndpointTests
{
    private readonly ApiHttpClientBroker _apiHttpClientBroker;

    public Ping_EndpointTests(ApiHttpClientBroker userApiHttpClientProfileBroker)
    {
        _apiHttpClientBroker = userApiHttpClientProfileBroker;
    }

    private readonly UserProfile inputUserProfile = new UserProfile
    {
        GameProfileLink = null,
        TwitchLink = null,
        GamerTag = null,
        DiscordId = "Something#0000",
        ProfileBio = "I pwn the noobs"
    };

    [Fact]
    public async Task TestingGetRequest_PingEndpoint()
    {
        var fetched = await _apiHttpClientBroker.GetByQuery<string>("ping");

        fetched.Should().BeEquivalentTo("pong");
    }


    [Fact]
    public async Task TestingPostRequest_PingEndpoint()
    {
        var testData = inputUserProfile;
        var fetchedData = await _apiHttpClientBroker.PostWithBody("ping", testData);

        fetchedData.TwitchLink.Should().BeEquivalentTo(testData.TwitchLink);
        fetchedData.GamerTag.Should().BeEquivalentTo(testData.GamerTag);
        fetchedData.DiscordId.Should().BeEquivalentTo(testData.DiscordId);
        fetchedData.ProfileBio.Should().BeEquivalentTo(testData.ProfileBio);
    }
}