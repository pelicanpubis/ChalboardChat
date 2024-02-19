//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace Chalkboardchat.Pages.Member
//{
//    public class DetailsModel : PageModel
//    {
//        public void OnGet()
//        {
//        }
//    }
//}


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Chalkboardchat.Pages.Member
{
    public class DetailsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }


        public DetailsModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager= signInManager;
        }

        //public DetailsModel(UserManager<IdentityUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                user.UserName = Username;
                var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, passwordToken, Password);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Member/Index");
                }
            }

            return Page();
        }
        public async Task<IActionResult> OnPostDeleteUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                // If user deletion is successful, sign out and redirect to login page
                await _signInManager.SignOutAsync();
                return RedirectToPage("/Account/Login");
            }
            else
            {
                // If user deletion fails, display any errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
        }
    }
}