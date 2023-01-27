using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Freelancer.ViewModels.User
{
    public class UserLoginVM
    {
        public string UserNameOrEmail { get; set; }
        [Microsoft.Build.Framework.Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistance { get; set; }
    }
}
