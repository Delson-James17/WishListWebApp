using System.ComponentModel.DataAnnotations;

namespace WishListWebApp.ViewModels
{
    public class LoginUserViewModel
    {
        // view for login

        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}