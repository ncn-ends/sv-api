using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using app;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using tests.UtilityTests.Brokers;

namespace tests.SystemTests.Brokers;

public class ApiHttpClientBroker
{
    private readonly WebApplicationFactory<Startup> _webApplicationFactory = new();
    private readonly HttpClient _baseClient;
    private const string _baseUri = "https://localhost:5001/";
    private readonly TestUserBroker testUser = new();

    public ApiHttpClientBroker()
    {
        _baseClient = _webApplicationFactory.CreateClient();

        _baseClient.BaseAddress = new Uri(_baseUri);
        _baseClient.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async ValueTask<T?> GetByQuery<T>(string query)
    {
        var res = await _baseClient.GetAsync($"{query}");
        res.EnsureSuccessStatusCode();
        var jsonString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
    
    public async ValueTask<bool> DeleteFromPath(string path)
    {
        await testUser.InitializeToken();

        _baseClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", testUser.BearerToken);

        var res = await _baseClient.DeleteAsync(path);
        res.EnsureSuccessStatusCode();
        return true;
    }

    public async ValueTask<T?> PostWithBody<T>(string path, T body)
    {
        await testUser.InitializeToken();

        _baseClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", testUser.BearerToken);

        var res = await _baseClient.PostAsJsonAsync(path, body);
        res.EnsureSuccessStatusCode();
        var jsonString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
    
    
    public async ValueTask<T?> PutWithBody<T>(string path, T body)
    {
        await testUser.InitializeToken();

        _baseClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", testUser.BearerToken);

        var res = await _baseClient.PutAsJsonAsync(path, body);
        res.EnsureSuccessStatusCode();
        var jsonString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
}