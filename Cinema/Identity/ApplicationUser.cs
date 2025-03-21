using Cinema.Models;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Identity
{
    public class ApplicationUser :IdentityUser
    {
        public string Address{ get; set; }
        public Cart  UserCart{ get; set; }
    }
}
