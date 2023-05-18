using WishListWebApp.Models;
using WishListWebApp.ViewModels;

namespace WishListWebApp.Data.Repositories
{
    public interface IWishlistRepository
    {
        Task<List<Wishlist>>GetAllWishlist(string token);
        Task<Wishlist> GetWishlistById(int wishid);
        Task<Wishlist?> CreateWishlist(WishlistViewModel newwishlist , string token);
        Task DeleteWishList(int wishid);
        Task<Wishlist?> UpdateWishlist(int wishid, Wishlist newwishlist);

    }
}
