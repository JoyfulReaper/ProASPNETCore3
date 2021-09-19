using Microsoft.AspNetCore.Mvc;
using SimpleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApp.Controllers
{
    public class HomeController : Controller
    {
        public IDataSource dataSource = new ProductDataSource();

        public IActionResult Index()
        {
            return View(dataSource.Products);
        }
    }
}
