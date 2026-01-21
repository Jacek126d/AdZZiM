using Microsoft.AspNetCore.Identity;

namespace AdZZiM.Models
{
    public class UserDetails
    {
        public IdentityUser User { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}