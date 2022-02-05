using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Components
{
    public class CitySummary : ViewComponent
    {
        private readonly CitiesData _data;

        public CitySummary(CitiesData data)
        {
            _data = data;
        }

        //public string Invoke()
        //{
        //    return $"{ _data.Cities.Count() } cities, " +
        //        $"{ _data.Cities.Sum(c => c.Population) } people";
        //}

        public IViewComponentResult Invoke()
        {
            return View(new CityViewModel
            {
                Cities = _data.Cities.Count(),
                Population = _data.Cities.Sum(c => c.Population),
            });
        }
    }
}
