using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Validation;

namespace WebApp.Models
{
    //[PhraseAndPrice(Phrase = "Small", Price = "100")]
    public class Product
    {
        public long ProductId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Column(TypeName = "decimal(8, 2)")]
        [Required(ErrorMessage = "Please enter a price")]
        [Range(1, 999999, ErrorMessage = "Please enter a positive price")]
        //[DisplayFormat(DataFormatString="{0:c2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [PrimaryKey(ContextType = typeof(DataContext), DataType = typeof(Category))]
        //[Remote("CategoryKey", "Validation", ErrorMessage = "Enter an existing key")]
        public long? CategoryId { get; set; }
        public Category? Category { get; set; }

        [PrimaryKey(ContextType = typeof(DataContext), DataType = typeof(Supplier))]
        //[Remote("SupplierKey", "Validation", ErrorMessage = "Enter an existing key")]
        public long? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
