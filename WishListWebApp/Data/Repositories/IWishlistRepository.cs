using WishListWebApp.Models;
using WishListWebApp.ViewModels;

namespace WishListWebApp.Data.Repositories
{
    public interface IWishlistRepository
    {
        Task<List<Wishlist>>GetAllWishlist(string token);
        Task<Wishlist> GetWishlistById(int wishid , string token);
        Task<Wishlist?> CreateWishlist(WishlistViewModel newwishlist , string token);
        Task DeleteWishList(int wishid , string token);
        Task<Wishlist?> UpdateWishlist(int wishid, Wishlist newwishlist, string token);

    }
}
