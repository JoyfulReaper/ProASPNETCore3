using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _order;
        private readonly Cart _cart;

        public OrderController(IOrderRepository order,
            Cart cart)
        {
            _order = order;
            _cart = cart;
        }

        public ViewResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (_cart.Lines.Count == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if(ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray();
                _order.SaveOrder(order);
                _cart.Clear();
                return RedirectToPage("/Completed", new { OrderId = order.OrderId });
            } 
            else
            {
                return View();
            }
        }
    }
}
