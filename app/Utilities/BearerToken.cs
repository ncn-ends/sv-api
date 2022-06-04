using System.Net.Http.Headers;
using app.Models;
using Newtonsoft.Json;

namespace app.Utilities;

public class TokenRequestBody
{
    public string client_id { get; set; }
    public string client_secret { get; set; }
    public string audience { get; set; }
    public string grant_type { get; set; }

    public string? scope { get; set; }
    public string? username { get; set; }
    public string? password { get; set; }
}
public class UserToken
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public string expires_in { get; set; }
    public string? scope { get; set; }
}

public class BearerToken
{
    private readonly HttpClient _client;
    private readonly Uri _url = new ("https://build-order-dev.us.auth0.com/oauth/token");
    private readonly TokenRequestBody _tokenRequestCredentials;

    public BearerToken(TokenRequestBody tokenRequestCredentials)
    {
        _tokenRequestCredentials = tokenRequestCredentials;

        var client = new HttpClient();
        client.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        _client = client;
    }

    public async Task<UserToken?> Fetch()
    {
        var res = await _client.PostAsJsonAsync(_url, _tokenRequestCredentials);
        res.EnsureSuccessStatusCode();
        var jsonString = await res.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<UserToken>(jsonString);
    }
}