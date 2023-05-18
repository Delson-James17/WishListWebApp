using WishListWebApp.Models;

namespace WishListWebApp.Data.Repositories
{
    public interface IWishlistRepository
    {
        Task<List<Wishlist>>GetAllWishlist();
        Task<Wishlist> GetWishlistById(int wishid);
        Task<Wishlist?> CreateWishlist(Wishlist newwishlist);
        Task DeleteWishList(int wishid);
        Task<Wishlist?> UpdateWishlist(int wishid, Wishlist newwishlist);

    }
}
