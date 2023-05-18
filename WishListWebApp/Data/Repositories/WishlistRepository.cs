using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WishListWebApp.Models;
using WishListWebApp.ViewModels;

namespace WishListWebApp.Data.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configs;
        private readonly string _baseURL;
        public WishlistRepository(IConfiguration configs) 
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(httpClientHandler);
            _configs = configs;
            _baseURL = "http://localhost:5211/api"; // Corrected base URL
        }
        public async Task<Wishlist?> CreateWishlist(WishlistViewModel newWishlist, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Assuming 'token' is defined

            string data = JsonConvert.SerializeObject(newWishlist);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var queryParameters = new Dictionary<string, string>
            {
                { "Name", newWishlist.Name },
                { "Description", newWishlist.Description },
                { "Price", newWishlist.Price },
                { "IsCompleted", newWishlist.IsCompleted },

            };

            string queryString = string.Join("&", queryParameters.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}")); // Fixed encoding of query parameters
            string fullURL = $"{_baseURL}/Wishlist?{queryString}";

            var response = _httpClient.PostAsync(fullURL, content).Result;
            if (response.IsSuccessStatusCode)
            {
                // Handle successful response here
            }
            return null;
        }

        public async Task DeleteWishList(int wishid)
        {
            var response = await _httpClient.DeleteAsync($"/wishlist/{wishid}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsByteArrayAsync();
                Console.WriteLine("Delete Wishlist Response: ", data);
            }
        }

        public async Task<List<Wishlist>> GetAllWishlist(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Assuming 'token' is defined
            string fullURL = $"{_baseURL}/Wishlist";

            var response = _httpClient.GetAsync(fullURL).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                var wishlist = JsonConvert.DeserializeObject<List<Wishlist>>(responseData);
                return wishlist;
            }
            else
            {
                return null;
            }
        }

        public async Task<Wishlist> GetWishlistById(int wishid)
        {
            var response = await _httpClient.GetAsync($"/wishlist/{wishid}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var wishlist = JsonConvert.DeserializeObject<Wishlist>(content);
                return wishlist;
            }

            return null;
        }

        public async Task<Wishlist?> UpdateWishlist(int wishid, Wishlist newwishlist)
        {
            var newWishlistAsString = JsonConvert.SerializeObject(newwishlist);
            var responseBody = new StringContent(newWishlistAsString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/wishlist/{wishid}", responseBody);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var wishlist = JsonConvert.DeserializeObject<Wishlist>(content);
                return wishlist;
            }

            return null;
        }
    }
}
