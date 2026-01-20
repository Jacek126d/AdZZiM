using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdZZiM.Models
{
    public class AccDate
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual IdentityUser? User { get; set; }
        [Display(Name ="Data rejestracji")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}