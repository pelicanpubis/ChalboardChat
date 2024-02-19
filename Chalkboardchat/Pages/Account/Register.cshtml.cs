using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chalkboardchat.Pages.Account
{

    [BindProperties]
    public class RegisterModel : PageModel
    {
    
            private readonly SignInManager<IdentityUser> _signInManager;
            private readonly UserManager<IdentityUser> _userManager;
            public string? Username { get; set; }

            public string? Password { get; set; }

            //Detta kommer från vår dependency injection
            public RegisterModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
            {
                _signInManager = signInManager;
                _userManager = userManager;
            }
            public void OnGet()
            {
            }

            public async Task<IActionResult> OnPost()
            {
                //Skapa en ny user med användarnamn och lösenord

                IdentityUser newUser = new()
                {
                    //Password ska inte vara med här
                    UserName = Username,

                };

                var createUserResult = await _userManager.CreateAsync(newUser, Password);

                if (createUserResult.Succeeded)

                {

                    //Lyckats skapa user
                    //Logga in

                    // UserService userService = new()
                    // userService.LogIn(Username, Password);

                    //hämtar den inlagda user från databasen
                    //Hämtar en user från databasen
                    IdentityUser? userLogin = await _userManager.FindByNameAsync(Username);

                    var signInResult = await _signInManager.PasswordSignInAsync(userLogin, Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        //Omdirigera användaren till members sidan

                        return RedirectToPage("/Member/Index");
                    }


                }

                else
                {
                    //Fel lösenord
                }

                return Page();

            }
        }
    }



