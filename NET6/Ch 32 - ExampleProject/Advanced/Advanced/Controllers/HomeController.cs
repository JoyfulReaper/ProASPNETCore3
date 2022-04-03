using Advanced.Models;
using Advanced.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Advanced.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        private readonly ToggleService _ts;

        public HomeController(DataContext context, ToggleService ts)
        {
            _context = context;
            _ts = ts;
        }

        public IActionResult Index([FromQuery]string selectedCity)
        {
            return View(new PeopleListViewModel
            {
                People = _context.People
                    .Include(p => p.Department)
                    .Include(p => p.Location),
                Cities = _context.Locations.Select(l => l.City).Distinct(),
                SelectedCity = selectedCity
            });
        }

        public string Toggle() => $"Enabled: {_ts.ToggleComponents() }";
    }

    public class PeopleListViewModel
    {
        public IEnumerable<Person> People { get; set; }
        public IEnumerable<string> Cities { get; set; }
        public string SelectedCity { get; set; }

        public string GetClass(string city) =>
            SelectedCity == city ? "bg-info text-white" : "";
    }
}
