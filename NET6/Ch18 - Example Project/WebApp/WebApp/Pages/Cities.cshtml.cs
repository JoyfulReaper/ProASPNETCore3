using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebApp.Models;

namespace WebApp.Pages
{
    [ViewComponent(Name = "CitiesPageHybrid")]
    public class CitiesModel : PageModel
    {
        public CitiesData Data { get; set; }

        [ViewComponentContext]
        public ViewComponentContext Context { get; set; }


        public CitiesModel(CitiesData data)
        {
            Data = data;
        }

        public void OnGet()
        {
        }

        public IViewComponentResult Invoke()
        {
            return new ViewViewComponentResult()
            {
                ViewData = new ViewDataDictionary<CityViewModel>(
                    Context.ViewData,
                    new CityViewModel
                    {
                        Cities = Data.Cities.Count(),
                        Population = Data.Cities.Sum(c => c.Population)
                    })
            };
        }
    }
}
