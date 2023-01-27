using Microsoft.AspNetCore.Identity;

namespace Freelancer.Models
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
