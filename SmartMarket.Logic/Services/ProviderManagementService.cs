using System.Text.Json;
using SmartMarket.Logic.Interfaces;
using SmartMarket.Logic.Models;

namespace SmartMarket.Logic.Services;

public class ProviderManagementService : IProviderManagementService
{
    private readonly HttpClient _client;

    public ProviderManagementService()
    {
        _client = new HttpClient();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _client.Dispose();
        }
    }

    public async Task<Provider?> GetFromApiByIdAsync(Guid id)
    {
        var response = await _client.GetAsync($"https://localhost:5001/api/providers/{id}");
        var responseContent = await response.Content.ReadAsStringAsync();
        var provider = JsonSerializer.Deserialize<Provider>(responseContent);
        return provider;
    }
}