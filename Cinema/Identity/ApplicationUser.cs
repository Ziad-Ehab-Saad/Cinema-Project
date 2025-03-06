using Microsoft.AspNetCore.Identity;

namespace Cinema.Identity
{
    public class ApplicationUser :IdentityUser
    {
        public string Address{ get; set; }

    }
}
