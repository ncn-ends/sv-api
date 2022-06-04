using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Models;
using FluentAssertions;
using tests.SystemTests.Brokers;
using Xunit;

namespace tests.SystemTests.EndpointTests;

[Collection(nameof(ApiTestCollection))]
public class Builds_EndpointTest
{
    private readonly ApiHttpClientBroker _apiHttpClientBroker;

    public Builds_EndpointTest(ApiHttpClientBroker userApiHttpClientProfileBroker)
    {
        _apiHttpClientBroker = userApiHttpClientProfileBroker;
    }

    private readonly BuildOrderListDto inputBuildOrderList = new()
    {
        Title = "title",
        Description = "description",
        Faction = "Protoss",
        Difficulty = "Expert",
        Tags = new List<TagDto>
        {
            new() {Label = "SOMETHING"}
        },
        BuildOrderSteps = new List<BuildOrderStepDto>
        {
            new() {WorkerCount = 5, GameDataId = 5},
            new() {WorkerCount = 10, GameDataId = 94}
        }
    };

    private readonly UserProfile inputUserProfile = new()
    {
        DiscordId = "Something#0000",
        ProfileBio = "I pwn the noobs"
    };

    [Fact]
    public async Task TestingBuildsEndpoint()
    {
        await _apiHttpClientBroker.PostWithBody("user/sync/profile", inputUserProfile);

        var submittedBuild = await _apiHttpClientBroker.PostWithBody("builds/new", inputBuildOrderList);

        var fetchedBuild = await _apiHttpClientBroker
            .GetByQuery<BuildOrderListDto>($"builds/one?Id={submittedBuild.BuildOrderListId}");

        fetchedBuild.Title.Should().BeEquivalentTo(submittedBuild.Title);
        fetchedBuild.Description.Should().BeEquivalentTo(submittedBuild.Description);
        fetchedBuild.Difficulty.Should().BeEquivalentTo(submittedBuild.Difficulty);
        fetchedBuild.Faction.Should().BeEquivalentTo(submittedBuild.Faction);
        fetchedBuild.Tags.Count.Should().Be(1);
        fetchedBuild.Tags.First().Label.Should().BeLowerCased();
        fetchedBuild.BuildOrderSteps.Count.Should().Be(2);

        /* clean up */
        await _apiHttpClientBroker.DeleteFromPath("user");
    }
}