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

        // Dependency Injection: AppDbContext anv�nds f�r att kommunicera med databasen.
        private readonly AppDbContext _context;

        // Konstruktor: Initierar en ny instans av IndexModel med en instans av AppDbContext.
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        // Egenskap: En lista av meddelanden som ska visas p� sidan.
        public IList<MessageModel> Messages { get; set; }

        // HTTP GET-metod: H�mtar alla meddelanden fr�n databasen n�r sidan laddas.
        public async Task<IActionResult> OnGetAsync()
        {
            Messages = await _context.Messages.ToListAsync();
            return Page();
        }

        // HTTP POST-metod: Anropas n�r anv�ndaren skickar ett nytt meddelande.
        // Meddelandet sparas i databasen och anv�ndaren omdirigeras till samma sida f�r att uppdatera inneh�llet.
        [BindProperty]
        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validera modellen innan behandling.
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // H�mta anv�ndarnamnet fr�n den autentiserade anv�ndaren.
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(username))
            {
                // Hantera fallet d�r anv�ndarnamnet inte �r tillg�ngligt.
                return BadRequest();
            }

            // Skapa en ny instans av MessageModel med anv�ndarinformation och meddelandeinneh�ll.
            var messageModel = new MessageModel
            {
                Date = DateTime.Now,
                Message = Message,
                Username = username
            };

            // L�gg till det nya meddelandet i databasen.
            _context.Messages.Add(messageModel);
            await _context.SaveChangesAsync();

            // Omdirigera anv�ndaren till sidan f�r att uppdatera inneh�llet.
            return RedirectToPage("./Index");
        }
    }
}


//public void OnGet()
//        {
//        }
//    }
//}
