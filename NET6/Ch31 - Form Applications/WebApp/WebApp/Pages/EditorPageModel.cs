﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages
{
    public class EditorPageModel : PageModel
    {
        protected readonly DataContext _context;

        public IEnumerable<Category> Categories => _context.Categories;
        public IEnumerable<Supplier> Suppliers => _context.Suppliers;

        public ProductViewModel ViewModel { get; set; }

        public EditorPageModel(DataContext context)
        {
            _context = context;
        }
    }
}
