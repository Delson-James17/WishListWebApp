using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using WishListWebApp.Models;

namespace WishListWebApp.Data.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        string BaseAddress = "http://localhost:5211/api";
        HttpClient httpClient = new HttpClient();
        public WishlistRepository() 
        { 

        }
        public async Task<Wishlist?> CreateWishlist(Wishlist newwishlist)
        {
            var newWishlistAsString = JsonConvert.SerializeObject(newwishlist);
            var responseBody = new StringContent(newWishlistAsString, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/wishlist", responseBody);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var wishlist = JsonConvert.DeserializeObject<Wishlist>(content);
                return wishlist;
            }

            return null;
        }

        public async Task DeleteWishList(int wishid)
        {
            var response = await httpClient.DeleteAsync($"/wishlist/{wishid}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsByteArrayAsync();
                Console.WriteLine("Delete Wishlist Response: ", data);
            }
        }

        public async Task<List<Wishlist>> GetAllWishlist()
        {
            var response = await httpClient.GetAsync(BaseAddress + "/wishlist");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var wishlists = JsonConvert.DeserializeObject<List<Wishlist>>(content);
                return wishlists ?? new();
            }

            return new();
        }

        public async Task<Wishlist> GetWishlistById(int wishid)
        {
            var response = await httpClient.GetAsync($"/wishlist/{wishid}");
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
            var response = await httpClient.PutAsync($"/wishlist/{wishid}", responseBody);
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
