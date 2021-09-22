using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        public int PageSize { get; set; } = 4;

        private readonly IStoreRepository _storeRepository;

        public HomeController(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public ViewResult Index(string category, int productPage = 1)
        {
            var vm = new ProductsListViewModel
            {
                Products = _storeRepository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductId)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                        _storeRepository.Products.Count() :
                        _storeRepository.Products.Where(
                            e => e.Category == category).Count()
                }
            };

            return View(vm);
        }
    }
}
