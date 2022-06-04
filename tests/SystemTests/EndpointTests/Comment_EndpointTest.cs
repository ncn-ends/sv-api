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
public class Comments_EndpointTest
{
    private readonly ApiHttpClientBroker _apiHttpClientBroker;

    public Comments_EndpointTest(ApiHttpClientBroker userApiHttpClientProfileBroker)
    {
        _apiHttpClientBroker = userApiHttpClientProfileBroker;
    }

    private readonly BuildOrderList inputBuildOrderList = new ()
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
    
    private readonly UserProfile inputUserProfile = new ()
    {
        DiscordId = "Something#0000",
        ProfileBio = "I pwn the noobs"
    };
    
    [Fact]
    public async Task TestingCommentEndpoint()
    {
        /*
         * Setting up user, build, and rating
         */
        await _apiHttpClientBroker.PostWithBody("user/sync/profile", inputUserProfile);
        var submittedBuild = await _apiHttpClientBroker.PostWithBody("builds/new", inputBuildOrderList);
        var initialCommentBody = new CommentEndpointBody
        {
            body = "initial body",
            listId = submittedBuild.BuildOrderListId
        };
        await _apiHttpClientBroker.PostWithBody("comment", initialCommentBody);
        
        /*
         * Checking that rating was added to the build
         */
        var fetchedBuild = await _apiHttpClientBroker
            .GetByQuery<BuildOrderListDto>($"builds/one?Id={submittedBuild.BuildOrderListId}");
        fetchedBuild.Comments.Count.Should().Be(1);
        fetchedBuild.Comments.First().Body.Should().Be(initialCommentBody.body);


        var commentId = fetchedBuild.Comments.First().CommentId;
        

        /*
         * Checking that rating was updated
         */
        var updatedCommentBody = new CommentEndpointBody
        {
            body = "after update",
            listId = submittedBuild.BuildOrderListId,
            commentId = commentId
        };
        await _apiHttpClientBroker.PutWithBody("comment", updatedCommentBody);
        var fetchedBuildAfterUpdate = await _apiHttpClientBroker
            .GetByQuery<BuildOrderListDto>($"builds/one?Id={submittedBuild.BuildOrderListId}");
        fetchedBuildAfterUpdate.Comments.Count.Should().Be(1);
        fetchedBuildAfterUpdate.Comments.First().Body.Should().Be(updatedCommentBody.body);

        /*
         * Checking that rating was deleted
         */
        await _apiHttpClientBroker.DeleteFromPath($"comment?commentId={commentId}");
        var fetchedBuildAfterDeletion = await _apiHttpClientBroker
            .GetByQuery<BuildOrderListDto>($"builds/one?Id={submittedBuild.BuildOrderListId}");
        fetchedBuildAfterDeletion.Comments.Count.Should().Be(0);
        
        /*
         * Cleaning up
         */
        await _apiHttpClientBroker.DeleteFromPath("user");
    }
}