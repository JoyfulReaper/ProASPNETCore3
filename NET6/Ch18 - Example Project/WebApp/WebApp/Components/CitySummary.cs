using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc.ViewComponents;

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

        //public IViewComponentResult Invoke()
        //{
        //    return View(new CityViewModel
        //    {
        //        Cities = _data.Cities.Count(),
        //        Population = _data.Cities.Sum(c => c.Population),
        //    });
        //}

        //public IViewComponentResult Invoke()
        //{
        //    return Content("This is a <h3><i>string</i></h3>");
        //}

        //public IViewComponentResult Invoke()
        //{
        //    return new HtmlContentViewComponentResult(
        //        new HtmlString("This is a <h3><i>string</i></h3>"));
        //}

        //public string Invoke()
        //{
        //    if(RouteData.Values["controller"] != null)
        //    {
        //        return "Controller Request";
        //    }
        //    else
        //    {
        //        return "Razor Page Request";
        //    }    
        //}

        public IViewComponentResult Invoke(string themeName)
        {
            ViewBag.Theme = themeName;

            return View(new CityViewModel
            {
                Cities = _data.Cities.Count(),
                Population = _data.Cities.Sum(x => x.Population)
            });
        }
    }
}
