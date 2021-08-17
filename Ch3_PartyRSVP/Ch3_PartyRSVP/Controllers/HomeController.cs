using Ch3_PartyRSVP.DataAccess;
using Ch3_PartyRSVP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ch3_PartyRSVP.Controllers
{
    public class HomeController : Controller
    {
        private readonly GuestResponseRepository _guestResponseRepository;

        public HomeController(GuestResponseRepository guestResponseRepository)
        {
            _guestResponseRepository = guestResponseRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult RsvpForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<ViewResult> RsvpForm(GuestResponse guestResponse)
        {
            if (ModelState.IsValid)
            {
                await _guestResponseRepository.AddAsync(guestResponse);
            }
            else
            {
                return View(guestResponse);
            }

            return View("Thanks", guestResponse);
        }
    }
}
