namespace PrintProxy.Hub.Services.Dto;

public class EditReservationDto
{
    public int ReservationId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public string UserId { get; set; }
    
    public int PrinterId { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
}