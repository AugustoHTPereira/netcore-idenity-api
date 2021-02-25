using Microsoft.AspNetCore.Identity;

namespace Supermercado.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
