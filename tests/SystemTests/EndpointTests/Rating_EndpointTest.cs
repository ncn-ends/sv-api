using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using app.Controllers;
using app.Models;
using FluentAssertions;
using tests.SystemTests.Brokers;
using Xunit;

namespace tests.SystemTests.EndpointTests;

[Collection(nameof(ApiTestCollection))]
public class Ratings_EndpointTest
{
    private readonly ApiHttpClientBroker _apiHttpClientBroker;

    public Ratings_EndpointTest(ApiHttpClientBroker userApiHttpClientProfileBroker)
    {
        _apiHttpClientBroker = userApiHttpClientProfileBroker;
    }

    private readonly BuildOrderList inputBuildOrderList = new()
    {
        Title = "title",
        Description = "description",
        Faction = "Protoss",
        Difficulty = "Expert",
        Tags = new List<Tag>
        {
            new() {Label = "SOMETHING"}
        }
    };
    
    private readonly UserProfile inputUserProfile = new()
    {
        DiscordId = "Something#0000",
        ProfileBio = "I pwn the noobs"
    };
    
    [Fact]
    public async Task TestingRatingEndpoint()
    {
        /*
         * Setting up user, build, and rating
         */
        await _apiHttpClientBroker.PostWithBody("user/sync/profile", inputUserProfile);
        var submittedBuild = await _apiHttpClientBroker.PostWithBody("builds/new", inputBuildOrderList);
        var initialRatingBody = new RatingEndpointBody
        {
            rating = true,
            listId = submittedBuild.BuildOrderListId
        };
        await _apiHttpClientBroker.PostWithBody("ratings", initialRatingBody);
        
        /*
         * Checking that rating was added to the build
         */
        var fetchedBuild = await _apiHttpClientBroker
            .GetByQuery<BuildOrderListDto>($"builds/one?Id={submittedBuild.BuildOrderListId}");
        fetchedBuild.Ratings.Count.Should().Be(1);
        fetchedBuild.Ratings.First().Value.Should().BeTrue();

        /*
         * Checking that rating was updated
         */
        var updatedRatingBody = new RatingEndpointBody
        {
            rating = false,
            listId = submittedBuild.BuildOrderListId
        };
        await _apiHttpClientBroker.PutWithBody("ratings", updatedRatingBody);
        var fetchedBuildAfterUpdate = await _apiHttpClientBroker
            .GetByQuery<BuildOrderListDto>($"builds/one?Id={submittedBuild.BuildOrderListId}");
        fetchedBuildAfterUpdate.Ratings.Count.Should().Be(1);
        fetchedBuildAfterUpdate.Ratings.First().Value.Should().BeFalse();
        
        /*
         * Checking that rating was deleted
         */
        await _apiHttpClientBroker.DeleteFromPath($"ratings?listId={submittedBuild.BuildOrderListId}");
        var fetchedBuildAfterDeletion = await _apiHttpClientBroker
            .GetByQuery<BuildOrderListDto>($"builds/one?Id={submittedBuild.BuildOrderListId}");
        fetchedBuildAfterDeletion.Ratings.Count.Should().Be(0);
        
        /*
         * Cleaning up
         */
        await _apiHttpClientBroker.DeleteFromPath("user");
    }
}