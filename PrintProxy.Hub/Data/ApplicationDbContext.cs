using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrintProxy.Hub.Components.Pages;
using PrintProxy.Hub.Data.Entities;

namespace PrintProxy.Hub.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        public DbSet<Printer> Printers { get; set; }
        
        public DbSet<Tag> Tags { get; set; }

        public DbSet<PrinterFile> Files { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
