using Microsoft.AspNetCore.Mvc;
using SimpleApp.Model;

namespace SimpleApp.Controllers
{
    public class HomeController : Controller
    {
        public IDataSource DataSource = new ProductDataSource();

        public ViewResult Index()
        {
            return View(Product.GetProducts());
        }

        public ViewResult Index2()
        {
            return View(DataSource.Products);
        }
    }
}
