using System.ComponentModel.DataAnnotations.Schema;

namespace PrintProxy.Hub.Data.Entities;

public class Reservation
{
    public int ReservationId { get; set; }
    
    public string UserId { get; set; } = string.Empty;
    
    public int PrinterId { get; set; }
    
    public required DateTime StartDate { get; set; }
    
    public required DateTime EndDate { get; set; }
    
    public required string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; } = string.Empty;
    
    public ApplicationUser User { get; set; } = null!;

    public Printer Printer { get; set; } = null!;
}