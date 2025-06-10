using E_Commerce.Domain.Entities;
using E_Commerce.Web.Services.Interfaces;
using E_Commerce.Web.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace E_Commerce.Web.Services.Implementations
{
    public class PaymobService : IPaymobService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _integrationId;
        private readonly string _currency;
        public PaymobService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["Paymob:ApiKey"];
            _integrationId = _configuration["Paymob:IntegrationId"];
            _currency = _configuration["Paymob:Currency"];
        }
        public async Task<string> GetAuthTokenAsync()
        {
            var requset = new { api_key = _apiKey };
            var requstContent = new StringContent(
                JsonSerializer.Serialize(requset),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PostAsync("https://accept.paymobsolutions.com/api/auth/tokens", requstContent);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonResponse);
            return doc.RootElement.GetProperty("token").GetString();
        }
        public async Task<string> CreateOrderAsync(string authToken, OrderVM order)
        {
            var requestContent = new
            {
                auth_token = authToken,
                delivery_needed = "false",
                amount_cents = order.Order.TotalPrice * 100,
                currency = _currency,
                items = new object[] { }// add items when i tried to add the orderitem here
            };

            //error accure here about the space is too much that is mean the orderitems is much of limit
            var content = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accept.paymobsolutions.com/api/ecommerce/orders")
            {
                Content = content
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("id").GetInt32().ToString();
        }
        public async Task<string> GetPaymentTokenAsync(string authToken, string orderId, OrderVM order)
        {
            var requestContent = new
            {
                auth_token = authToken,
                amount_cents = order.Order.TotalPrice * 100,
                expiration = 3600,
                order_id = int.Parse(orderId),
                billing_data = new
                {
                    apartment = "NA",
                    email = order.User.Email,
                    first_name = "Test",
                    last_name = "Test",
                    floor = order.User.Address,
                    street = order.User.Address,
                    building = order.User.Address,
                    phone_number = order.User.PhoneNumber,
                    shipping_method = "NA",
                    postal_code = "12345",
                    city = order.User.Address,
                    country = "EG",
                    state = order.User.Address
                },
                currency = _currency,
                integration_id = int.Parse(_integrationId),
                lock_order_when_paid = false
            };

            var content = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://accept.paymobsolutions.com/api/acceptance/payment_keys", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("token").GetString();
        }


        public async Task<bool> IsPaymentSuccessfully(string orderId, string authToken)
        {
            var url = "https://accept.paymob.com/api/ecommerce/orders/transaction_inquiry";
            var content = new { order_id = orderId };
            var requstContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
            var requst = new HttpRequestMessage(HttpMethod.Post, url);
            requst.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            requst.Content = requstContent;
            var response = await _httpClient.SendAsync(requst);

            if (!response.IsSuccessStatusCode)
                return false;

            var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                using var doc = JsonDocument.Parse(responseContent);
                bool success = doc.RootElement.TryGetProperty("success", out var successElement) && successElement.GetBoolean();
                string paymentStatus = "";
                if (doc.RootElement.TryGetProperty("order", out var orderElement) &&
                    orderElement.TryGetProperty("payment_status", out var statusElement))
                {
                    paymentStatus = statusElement.GetString();
                }
                return success && paymentStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
