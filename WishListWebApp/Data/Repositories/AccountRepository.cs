using Microsoft.AspNetCore.Identity;
using WishListWebApp.Models;
using WishListWebApp.ViewModels;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http;
using System.IdentityModel.Tokens.Jwt;

namespace WishListWebApp.Repository
{
    public class AccountRepository : IAccountRepository
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configs;

        public AccountRepository(IConfiguration configs)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(httpClientHandler);

            _configs = configs;
            _httpClient.BaseAddress = new Uri("http://localhost:5211/api");
        }

        public async Task<string> SignInUserAsync(LoginUserViewModel loginUserViewModel)
        {
            // Add this code before making the request
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            var newWishAsString = JsonConvert.SerializeObject(loginUserViewModel);
            var requestBody = new StringContent(newWishAsString, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            var response = await _httpClient.PostAsync("/Login", requestBody);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                // extract token from responce and store it in session
                var token = JObject.Parse(content)["token"].ToString();

                return token;
            }

            return null;
        }

        public async Task<bool> SignUpUserAsync(RegisterUserViewModel user)
        {

            var newTodoAsString = JsonConvert.SerializeObject(user);
            var requestBody = new StringContent(newTodoAsString, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _configs.GetValue<string>("ApiKey"));
            var response = await _httpClient.PostAsync("/Signup", requestBody);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return true;
            }

            return false;
        }
        public async Task<string> GetApplicationUserId(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            foreach (var claim in jwtToken.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            // var applicationUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            return null;
        }


    }
}
