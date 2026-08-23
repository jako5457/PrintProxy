namespace PrintProxy.Hub.Services.Dto;

public class ReservationInfoDto
{
    
    public int ReservationId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public string UserName { get; set; }
    
    public string PrinterName { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
}