using System.ComponentModel.DataAnnotations;

namespace AdZZiM.Models
{
    public class CreateUser
    {
        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email")]
        [Display(Name = "Adres Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public required string Password { get; set; }
    }
}