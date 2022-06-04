using System.Threading.Tasks;
using app.Models;
using FluentAssertions;
using tests.SystemTests.Brokers;
using Xunit;

namespace tests.SystemTests.EndpointTests;

[Collection(nameof(ApiTestCollection))]
public class User_EndpointTest
{
    private readonly ApiHttpClientBroker _apiHttpClientBroker;

    public User_EndpointTest(ApiHttpClientBroker userApiHttpClientProfileBroker)
    {
        _apiHttpClientBroker = userApiHttpClientProfileBroker;
    }

    private readonly UserProfile inputUserProfile = new()
    {
        DiscordId = "Something#0000",
        ProfileBio = "I pwn the noobs"
    };

    [Fact]
    public async Task TestingUserSyncEndpoint()
    {
        await _apiHttpClientBroker.DeleteFromPath("user");
        var syncedUser = await _apiHttpClientBroker.PostWithBody("user/sync/profile", inputUserProfile);
        
        var fetchedUser = await _apiHttpClientBroker.GetByQuery<UserProfile>($"user?id={syncedUser.UserProfileId}");
        
        fetchedUser.GameProfileLink.Should().BeEquivalentTo(syncedUser.GameProfileLink);
        fetchedUser.TwitchLink.Should().BeEquivalentTo(syncedUser.TwitchLink);
        fetchedUser.GamerTag.Should().BeEquivalentTo(syncedUser.GamerTag);
        fetchedUser.DiscordId.Should().BeEquivalentTo(syncedUser.DiscordId);
        fetchedUser.ProfileBio.Should().BeEquivalentTo(syncedUser.ProfileBio);

        await _apiHttpClientBroker.DeleteFromPath("user");
    }
    
    [Fact]
    public async Task TestingUserRegisterEndpoint()
    {
        // TODO: this
    }
}