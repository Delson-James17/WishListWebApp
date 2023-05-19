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
          var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case when the token is not available
                return RedirectToAction("Login", "Account");
            }
            var viewModel = new IndexViewModel
            {

                Wishlists = await _wishlistRepository.GetAllWishlist(token)
            };

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(WishlistViewModel newWishlist)
        {
            var token = HttpContext.Session.GetString("JWToken");
            //newWishlist.Id = 1;
            newWishlist.IsCompleted = "true";

            await _wishlistRepository.CreateWishlist(newWishlist, token);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult>Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var wishlist = await _wishlistRepository.GetWishlistById(id, token);
            if (wishlist == null)
                return NotFound();

            return View(wishlist);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Wishlist updatedWishlist)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (id != updatedWishlist.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var modifiedWishlist = await _wishlistRepository.UpdateWishlist(id, updatedWishlist, token);
                if (modifiedWishlist != null)
                {
                    if (modifiedWishlist.IsCompleted == "true")
                    {
                        return RedirectToAction(nameof(Index), new { id = modifiedWishlist.Id, token });
                    }
                    else
                    {
                        // Handle the case where the update was not successful
                        // You can display an error message or perform other actions as per your requirement
                        ViewBag.ErrorMessage = "Failed to update the wishlist.";
                        return View(updatedWishlist);
                    }
                }
                else
                {
                    // Handle the case where the update operation returned null
                    // You can display an error message or perform other actions as per your requirement
                    ViewBag.ErrorMessage = "Failed to update the wishlist.";
                    return View(updatedWishlist);
                }
            }

            return View(updatedWishlist);
        }



        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            await _wishlistRepository.DeleteWishList(id, token);
            return RedirectToAction("Index");
        }
    


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}