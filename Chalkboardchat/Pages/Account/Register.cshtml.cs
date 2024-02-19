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

            //Detta kommer fr�n v�r dependency injection
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
                //Skapa en ny user med anv�ndarnamn och l�senord

                IdentityUser newUser = new()
                {
                    //Password ska inte vara med h�r
                    UserName = Username,

                };

                var createUserResult = await _userManager.CreateAsync(newUser, Password);

                if (createUserResult.Succeeded)

                {

                    //Lyckats skapa user
                    //Logga in

                    // UserService userService = new()
                    // userService.LogIn(Username, Password);

                    //h�mtar den inlagda user fr�n databasen
                    //H�mtar en user fr�n databasen
                    IdentityUser? userLogin = await _userManager.FindByNameAsync(Username);

                    var signInResult = await _signInManager.PasswordSignInAsync(userLogin, Password, false, false);

                    if (signInResult.Succeeded)
                    {
                        //Omdirigera anv�ndaren till members sidan

                        return RedirectToPage("/Member/Index");
                    }


                }

                else
                {
                    //Fel l�senord
                }

                return Page();

            }
        }
    }



