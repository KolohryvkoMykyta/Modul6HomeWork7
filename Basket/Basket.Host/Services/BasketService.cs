using Basket.Host.Models;
using Basket.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Basket.Host.Services;

public class BasketService : IBasketService
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<BasketService> _logger;

    public BasketService(ICacheService cacheService, ILogger<BasketService> logger)
    {
        _logger = logger;
        _cacheService = cacheService;
    }
    
    public async Task AddItem(string userId, BasketItem data)
    {
        bool isExistingData;
        var list = new List<BasketItem>() { data };

        var existingData = await _cacheService.GetAsync<List<BasketItem>>(userId);

        if (existingData != null)
        {
            isExistingData = true;
            existingData.AddRange(list);
        }
        else
        {
            isExistingData= false;
            existingData = list;
        }

        await _cacheService.AddOrUpdateAsync(userId, existingData);
        
        if (isExistingData) 
        {
            _logger.LogInformation($"Cached value for key {userId} updated"); 
        }
        else
        {
            _logger.LogInformation($"Cached value for key {userId} cached");
        }
    }

    public async Task RemoveItem(string userId, int itemId)
    {
        var existingData = await _cacheService.GetAsync<List<BasketItem>>(userId);

        if (existingData != null)
        {
            var item = existingData.Where(i => i.Id == itemId).FirstOrDefault();
            
            if (item != null) 
            {
                existingData.Remove(item);
                await _cacheService.AddOrUpdateAsync(userId, existingData);
                _logger.LogInformation($"Remove item {itemId}");
            }
            else
            {
                _logger.LogInformation($"No such id {itemId}");
            }
        }
        else
        {
            _logger.LogInformation($"No data for {userId} key");
        }
    }

    public async Task<GetResponse> GetItems(string userId)
    {
        var result = await _cacheService.GetAsync<List<BasketItem>>(userId);
        return new GetResponse() { Data = result ?? new List<BasketItem>() };
    }

    public async Task ClearData(string userId)
    {
        await _cacheService.ClearData(userId);
    }
}