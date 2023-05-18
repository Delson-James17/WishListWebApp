using Microsoft.AspNetCore.Identity;
using WishListWebApp.Models;
using WishListWebApp.ViewModels;

namespace WishListWebApp.Repository
{
    public interface IAccountRepository
    {
        Task<bool> SignUpUserAsync(RegisterUserViewModel user);
        Task<string> SignInUserAsync(LoginUserViewModel loginUserViewModel);
        Task<string> GetApplicationUserId(string token);
    }
}
