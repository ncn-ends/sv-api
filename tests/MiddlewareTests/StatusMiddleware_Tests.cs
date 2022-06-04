using System;
using System.IO;
using System.Threading.Tasks;
using app;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace tests;

public class StatusMiddleware_Tests
{
    [Fact]
    public async Task ForNonMatchingRequest_CallsNextDelegate()
    {
        var context = new DefaultHttpContext();
        context.Request.Path = "/somethingelse";
        var wasExecuted = false;

        RequestDelegate next = (HttpContext _) =>
        {
            wasExecuted = true;
            return Task.CompletedTask;
        };
        var middleware = new StatusMiddleware(next);
        await middleware.Invoke(context);
        Assert.True(wasExecuted);
    }

    [Fact]
    public async Task ReturnsPongBodyContent()
    {
        var bodyStream = new MemoryStream();
        var context = new DefaultHttpContext();
        context.Response.Body = bodyStream;
        context.Request.Path = "/ping";
        RequestDelegate next = (ctx) => Task.CompletedTask;
        var middleware = new StatusMiddleware(next: next);
        await middleware.Invoke(context);

        string response;
        bodyStream.Seek(0, SeekOrigin.Begin);
        using var stringReader = new StreamReader(bodyStream);
        response = await stringReader.ReadToEndAsync();
        Assert.Equal("pong", response);
        Assert.Equal("text/plain", context.Response.ContentType);
        Assert.Equal(200, context.Response.StatusCode);
    }
}