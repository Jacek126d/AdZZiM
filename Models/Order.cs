using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdZZiM.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Data zamówienia")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string UserId { get; set; } = "";
        public virtual IdentityUser? User { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany")]
        [StringLength(200, ErrorMessage = "Adres jest za długi")]
        public string DeliveryAddress { get; set; } = "";

        [Display(Name = "Metoda wysyłki")]
        public string ShippingMethod { get; set; } = "Kurier";
        [Display(Name = "Metoda płatności")]
        public string PaymentMethod { get; set; } = "Przy odbiorze";

        [Display(Name = "Status")]
        public string Status { get; set; } = "W realizacji";
        [Display(Name = "Łączna kwota")]
        [Column(TypeName = "decimal(18,2)")] public decimal TotalAmount { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}