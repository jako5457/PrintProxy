using Microsoft.EntityFrameworkCore;
using PrintProxy.Hub.Data;
using PrintProxy.Hub.Services.Dto;

namespace PrintProxy.Hub.Services;

public class ReservationService
{
    
    private readonly ILogger<ReservationService> _logger;

    private readonly IServiceProvider _serviceProvider;

    public ReservationService(ILogger<ReservationService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<ReservationInfoDto?> GetCurrentPrinterReservationAsync(int printerId)
    {
       await using var scope = _serviceProvider.CreateAsyncScope();

       ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

       return await context.Reservations
                            .Where(r => r.Printer.PrinterId == printerId)
                            .Where(r => DateTime.Now > r.StartDate)
                            .Where(r => DateTime.Now < r.EndDate)
                            .Select(r => new ReservationInfoDto()
                            {
                                ReservationId = r.ReservationId,
                                StartDate = r.StartDate,
                                EndDate = r.EndDate,
                                Title =  r.Title,
                                Description =  r.Description,
                                UserName = r.User.UserName ?? "none",
                            })
                            .FirstOrDefaultAsync();
       
    }

    public async Task<List<ReservationInfoDto>> GetReservationsByPrinterAsync(int printerId)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Reservations
            .Where(r => r.Printer.PrinterId == printerId)
            .Select(r => new ReservationInfoDto()
            {
                ReservationId = r.ReservationId,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Title =  r.Title,
                Description =  r.Description,
                UserName = r.User.UserName ?? "none",
            })
            .ToListAsync();
    }

    public async Task<List<ReservationInfoDto>> GetReservationsByUserAsync(string userId)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Reservations
            .Where(r => r.User.Id == userId)
            .Select(r => new ReservationInfoDto()
            {
                ReservationId = r.ReservationId,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Title =  r.Title,
                Description =  r.Description,
                UserName = r.User.UserName ?? "none",
            })
            .ToListAsync();
    }
    
}