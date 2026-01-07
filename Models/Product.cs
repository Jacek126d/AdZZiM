using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdZZiM.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa produktu jest wymagana")]
        public string Name { get; set; } = "";

        [Range(0.01, 10000, ErrorMessage = "Cena musi być większa od 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}