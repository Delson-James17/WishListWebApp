using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WishListWebApp.Data.Repositories;
using WishListWebApp.Models;
using WishListWebApp.ViewModels;

namespace WishListWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWishlistRepository _wishlistRepository;

        public HomeController(ILogger<HomeController> logger , IWishlistRepository wishlistRepository)
        {
            _logger = logger;
            _wishlistRepository = wishlistRepository;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel
            {
                Wishlists = await _wishlistRepository.GetAllWishlist()
            };

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Wishlist newWishlist)
        {
            newWishlist.Id = 1;
            newWishlist.IsCompleted = false;

            await _wishlistRepository.CreateWishlist(newWishlist);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int wishId)
        {
            var wish = await _wishlistRepository.GetWishlistById(wishId);

            if (wish is null)
                return NotFound();

            return View(wish);
        }
        [HttpPost]
        public async Task<IActionResult>Edit(Wishlist updatewishlist)
        {
            var test = await _wishlistRepository.UpdateWishlist(updatewishlist.Id, updatewishlist);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int wishId)
        {
            await _wishlistRepository.DeleteWishList(wishId);
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}