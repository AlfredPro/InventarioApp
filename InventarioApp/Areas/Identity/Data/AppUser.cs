using Microsoft.AspNetCore.Identity;

namespace InventarioApp.Areas.Identity.Data
{
    public class AppUser : IdentityUser
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

    }
}
