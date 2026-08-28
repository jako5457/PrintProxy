using Microsoft.AspNetCore.Identity;
using PrintProxy.Hub.Data.Entities;

namespace PrintProxy.Hub.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {

        public List<Reservation> Reservations { get; set; } = null!;

    }

}
