using Chalkboardchat.Data.Database;
using Chalkboardchat.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chalkboardchat.Pages.Member
{
    public class IndexModel : PageModel
    {

        // Dependency Injection: AppDbContext används för att kommunicera med databasen.
        private readonly AppDbContext _context;

        // Konstruktor: Initierar en ny instans av IndexModel med en instans av AppDbContext.
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        // Egenskap: En lista av meddelanden som ska visas på sidan.
        public IList<MessageModel> Messages { get; set; }

        // HTTP GET-metod: Hämtar alla meddelanden från databasen när sidan laddas.
        public async Task<IActionResult> OnGetAsync()
        {
            Messages = await _context.Messages.ToListAsync();
            return Page();
        }

        // HTTP POST-metod: Anropas när användaren skickar ett nytt meddelande.
        // Meddelandet sparas i databasen och användaren omdirigeras till samma sida för att uppdatera innehållet.
        [BindProperty]
        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validera modellen innan behandling.
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Hämta användarnamnet från den autentiserade användaren.
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(username))
            {
                // Hantera fallet där användarnamnet inte är tillgängligt.
                return BadRequest();
            }

            // Skapa en ny instans av MessageModel med användarinformation och meddelandeinnehåll.
            var messageModel = new MessageModel
            {
                Date = DateTime.Now,
                Message = Message,
                Username = username
            };

            // Lägg till det nya meddelandet i databasen.
            _context.Messages.Add(messageModel);
            await _context.SaveChangesAsync();

            // Omdirigera användaren till sidan för att uppdatera innehållet.
            return RedirectToPage("./Index");
        }
    }
}


//public void OnGet()
//        {
//        }
//    }
//}
