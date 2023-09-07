using MVC.Models.Requests;
using MVC.Models.Responses;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services
{
    public class BasketService : IBasketService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly IHttpClientService _httpClient;
        private readonly ILogger<BasketService> _logger;

        public BasketService(IHttpClientService httpClient, ILogger<BasketService> logger, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;
        }

        public async Task AddBasketItem(int id, string name, decimal price)
        {
            await _httpClient.SendAsync<object, AddBasketItemRequest>($"{_settings.Value.BasketUrl}/addItem",
           HttpMethod.Post,
           new AddBasketItemRequest() { Id = id, Name = name, Price = price });
        }

        public async Task RemoveBasketItem(int id)
        {
            await _httpClient.SendAsync<object, RemoveBasketItemRequest>($"{_settings.Value.BasketUrl}/removeItem",
           HttpMethod.Post,
           new RemoveBasketItemRequest() { Id = id });
        }

        public async Task<IReadOnlyList<BasketItem>> GetBasketItems()
        {
            var result = await _httpClient.SendAsync<GetBasketResponse, string>($"{_settings.Value.BasketUrl}/getItems",
                HttpMethod.Post,
                default);

            if (result != null) 
            { 
                return result.Data; 
            }

            return new List<BasketItem>();
        }

        public async Task ClearData()
        {
            await _httpClient.SendAsync<object, string> ($"{_settings.Value.BasketUrl}/clearData",
                HttpMethod.Post,
                default);
        }
    }
}
