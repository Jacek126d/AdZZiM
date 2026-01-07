using System.ComponentModel.DataAnnotations;

namespace AdZZiM.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa kategorii jest wymagana")]
        [Display(Name = "Kategoria")]
        public string Name { get; set; } = "";

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}