using System;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Service;

public class AuctionSvcHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

      public async Task<List<Item>> GetItemsForSearchDb()
    {
var lastRecord = await DB.Find<Item>()
        .Sort(x => x.Descending(x => x.UpdatedAt))
        .ExecuteFirstAsync();   

    // Nếu DB không có bản ghi
    if (lastRecord == null)
    {
        Console.WriteLine("No existing items → fetching all auctions");

        return await _httpClient.GetFromJsonAsync<List<Item>>(
            $"{_configuration["AuctionServiceUrl"]}/api/auctions"
        );
    }

    var lastUpdated = lastRecord.UpdatedAt;

    
    var iso = Uri.EscapeDataString(lastUpdated.ToUniversalTime().ToString("o"));

    
    return await _httpClient.GetFromJsonAsync<List<Item>>(
        $"{_configuration["AuctionServiceUrl"]}/api/auctions?date={iso}"
    );

          
    }
}
