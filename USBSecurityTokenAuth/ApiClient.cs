using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using USBSecurityTokenAuth.Models;

namespace USBSecurityTokenAuth;

public class ApiClient
{
    private readonly HttpClient _client;

    private readonly string _apiUrl;

    public ApiClient(IConfiguration configuration)
    {
        _client = new HttpClient();
        _apiUrl = configuration["API:Url"];
    }

    public async Task<string> SendSignInRequest(SignInRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8,
            "application/json");
        var response = await _client.PostAsync(_apiUrl + "identity/signIn", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> SendRegisterRequest(NewUser user)
    {
        var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(_apiUrl + "Identity/register", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}