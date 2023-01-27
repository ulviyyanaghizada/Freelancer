using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Freelancer.ViewModels.User
{
    public class UserRegisterVM
    {
        [Microsoft.Build.Framework.Required]
        public string Name { get; set; }
        [Microsoft.Build.Framework.Required]
        public string Surname { get; set; }
        [Microsoft.Build.Framework.Required]
        public string Username { get; set; }
        [Microsoft.Build.Framework.Required, DataType(DataType.EmailAddress)]
        
        public string Email { get; set; }
        [Microsoft.Build.Framework.Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Microsoft.Build.Framework.Required, DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
