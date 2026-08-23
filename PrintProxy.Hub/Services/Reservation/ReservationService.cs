using System.Globalization;
using Microsoft.EntityFrameworkCore;
using PrintProxy.Hub.Data;
using PrintProxy.Hub.Data.Entities;
using PrintProxy.Hub.Services.Dto;
using PrintProxy.Hub.Services.reservation;

namespace PrintProxy.Hub.Services;

public class ReservationService : IReservationService
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
                                StartDate = r.StartDate.ToLocalTime(),
                                EndDate = r.EndDate.ToLocalTime(),
                                Title =  r.Title,
                                Description =  r.Description,
                                UserName = r.User.UserName ?? "none",
                                PrinterName = r.Printer.PrinterName
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
                StartDate = r.StartDate.ToLocalTime(),
                EndDate = r.EndDate.ToLocalTime(),
                Title =  r.Title,
                Description =  r.Description,
                UserName = r.User.UserName ?? "none",
                PrinterName = r.Printer.PrinterName
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
                StartDate = r.StartDate.ToLocalTime(),
                EndDate = r.EndDate.ToLocalTime(),
                Title =  r.Title,
                Description =  r.Description,
                UserName = r.User.UserName ?? "none",
                PrinterName = r.Printer.PrinterName
            })
            .ToListAsync();
    }

    public async Task CreateReservationAsync(EditReservationDto reservation)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        Reservation newRes = new Reservation()
        {
            UserId = reservation.UserId,
            ReservationId = reservation.ReservationId,
            StartDate = reservation.StartDate.ToUniversalTime(),
            EndDate = reservation.EndDate.ToUniversalTime(),
            Title = reservation.Title,
            Description = reservation.Description,
        };
        
        context.Reservations.Add(newRes);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message,e);
            throw;
        }
    }

    public async Task CreateReservationsAsync(List<EditReservationDto> reservations)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        List<Reservation> NewReservations = reservations.Select(r => new Reservation()
        {
            UserId = r.UserId,
            PrinterId = r.PrinterId,
            StartDate = r.StartDate.ToUniversalTime(),
            EndDate = r.EndDate.ToUniversalTime(),
            Title = r.Title,
            Description = r.Description,
        }).ToList();
        
        context.Reservations.AddRange(NewReservations);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message,e);
            throw;
        }
    }

    public async Task EditReservationAsync(EditReservationDto reservation, int reservationId)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        var existingReservation = await context.Reservations
                                                .Where(r => r.ReservationId == reservationId)
                                                .FirstOrDefaultAsync();
        
        if (existingReservation != null)
        {
            existingReservation.Title = reservation.Title;
            existingReservation.Description = reservation.Description;
            existingReservation.StartDate = reservation.StartDate.ToUniversalTime();
            existingReservation.EndDate = reservation.EndDate.ToUniversalTime();
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            throw;
        }
    }

    public async Task DeleteReservationAsync(int reservationId)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
        
        var reservation = await context.Reservations.Where(r => r.ReservationId == reservationId).FirstOrDefaultAsync();

        if (reservation != null)
        {
            context.Reservations.Remove(reservation);
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            throw;
        }
    }

    public async Task<bool> ValidateValidReservationAsync(int printerId,DateTime start, DateTime end)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        return !await context.Reservations
            .Where(r => r.PrinterId == printerId)
            .Where(r => start.ToUniversalTime() <= r.StartDate.ToUniversalTime())
            .Where(r => end.ToUniversalTime() >= r.EndDate.ToUniversalTime()).AnyAsync();
    }
}
