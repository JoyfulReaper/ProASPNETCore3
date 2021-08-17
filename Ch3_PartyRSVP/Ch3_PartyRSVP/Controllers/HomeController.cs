using Ch3_PartyRSVP.DataAccess;
using Ch3_PartyRSVP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

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
                try
                {
                    await _guestResponseRepository.AddAsync(guestResponse);
                }
                catch (ArgumentException e)
                {
                    ModelState.AddModelError(string.Empty, "You have already RSVPed.");
                    return View(guestResponse);
                }
            }
            else
            {
                return View(guestResponse);
            }

            return View("Thanks", guestResponse);
        }
    }
}
