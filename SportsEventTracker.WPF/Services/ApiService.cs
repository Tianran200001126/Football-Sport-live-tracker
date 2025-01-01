
using System.Net.Http;
using System.Net.Http.Json;
using SportsEventTracker.Models;

namespace SportsEventTracker.WPF.Services
{
  public class ApiService
  {

    private readonly HttpClient _client;

    public ApiService()
    {
        _client = new HttpClient {BaseAddress = new Uri("http://localhost:5083/api/")};

    }

    public async Task<List<Team>> GetTeamsAsync()

    {

        var response =   await _client.GetFromJsonAsync<List<Team>>("team");
        if (response == null)
       {
        throw new Exception("Failed to fetch matches.");
       }
        return response;
    }

    public async Task<List<GameMatch>> GetMatchesAsync()
{
    var response = await _client.GetFromJsonAsync<List<GameMatch>>("matches");
    if (response == null)
    {
        throw new Exception("Failed to fetch matches.");
    }
    return response;
}

public async Task AddTeamAsync(Team team)
{
    try
    {
        var response = await _client.PostAsJsonAsync("team", team);

        if (response.IsSuccessStatusCode)
        {
            // Successfully added the team
            return;
        }

        throw new Exception($"Unexpected error: {response.StatusCode}");
    }
    catch (HttpRequestException ex)
    {
        throw new Exception("Error communicating with the server.", ex);
    }
}

  }
}