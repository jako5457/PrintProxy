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
        using var scope = _serviceProvider.CreateAsyncScope();

       try
       {
           ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
           await Task.Delay(Random.Shared.Next(100, 500));
           
           var utcNow = DateTime.UtcNow;
           
           return await context.Reservations
               .Where(r => r.Printer.PrinterId == printerId)
               .Where(r => r.StartDate <= utcNow)
               .Where(r => r.EndDate >= utcNow)
               .Select(r => new ReservationInfoDto()
               {
                   ReservationId = r.ReservationId,
                   StartDate = TimeZoneInfo.ConvertTime(r.StartDate,TimeZoneInfo.Local),
                   EndDate = TimeZoneInfo.ConvertTime(r.EndDate,TimeZoneInfo.Local),
                   Title =  r.Title,
                   PrinterId = r.Printer.PrinterId,
                   Description =  r.Description,
                   UserName = r.User.UserName ?? "none",
                   PrinterName = r.Printer.PrinterName
               })
               .FirstOrDefaultAsync();
       }
       catch (Exception e)
       {
           Console.WriteLine(e);
           return null;
       }
    }

    public async Task<List<ReservationInfoDto>> GetReservationsByPrinterAsync(int printerId)
    {
        using var scope = _serviceProvider.CreateAsyncScope();

        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Reservations
            .Where(r => r.Printer.PrinterId == printerId)
            .Select(r => new ReservationInfoDto()
            {
                ReservationId = r.ReservationId,
                StartDate = TimeZoneInfo.ConvertTime(r.StartDate,TimeZoneInfo.Local),
                EndDate = TimeZoneInfo.ConvertTime(r.EndDate,TimeZoneInfo.Local),
                Title =  r.Title,
                PrinterId = r.Printer.PrinterId,
                Description =  r.Description,
                UserName = r.User.UserName ?? "none",
                PrinterName = r.Printer.PrinterName
            })
            .ToListAsync();
    }

    public async Task<List<ReservationInfoDto>> GetReservationsByUserAsync(string userId)
    {
        using var scope = _serviceProvider.CreateAsyncScope();

        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Reservations
            .Where(r => r.User.Id == userId)
            .Select(r => new ReservationInfoDto()
            {
                ReservationId = r.ReservationId,
                StartDate = TimeZoneInfo.ConvertTime(r.StartDate,TimeZoneInfo.Local),
                EndDate = TimeZoneInfo.ConvertTime(r.EndDate,TimeZoneInfo.Local),
                Title =  r.Title,
                PrinterId = r.Printer.PrinterId,
                Description =  r.Description,
                UserName = r.User.UserName ?? "none",
                PrinterName = r.Printer.PrinterName
            })
            .ToListAsync();
    }
    
    public async Task<List<ReservationInfoDto>> GetReservationsByDateAsync(DateTime date)
    {
        using var scope = _serviceProvider.CreateAsyncScope();

        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
        
        return await context.Reservations
            .Where(r => r.StartDate <= TimeZoneInfo.ConvertTime(date,TimeZoneInfo.Utc) && TimeZoneInfo.ConvertTime(date,TimeZoneInfo.Utc) <= r.EndDate)
            .OrderByDescending(r => r.StartDate)
            .Select(r => new ReservationInfoDto()
            {
                ReservationId = r.ReservationId,
                StartDate = TimeZoneInfo.ConvertTime(r.StartDate,TimeZoneInfo.Local),
                EndDate = TimeZoneInfo.ConvertTime(r.EndDate,TimeZoneInfo.Local),
                Title =  r.Title,
                PrinterId = r.Printer.PrinterId,
                Description =  r.Description,
                UserName = r.User.UserName ?? "none",
                PrinterName = r.Printer.PrinterName
            })
            .ToListAsync();
    }
    
    public async Task CreateReservationAsync(EditReservationDto reservation)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        Reservation newRes = new Reservation()
        {
            UserId = reservation.UserId,
            ReservationId = reservation.ReservationId,
            StartDate = TimeZoneInfo.ConvertTime(reservation.StartDate,TimeZoneInfo.Utc),
            EndDate = TimeZoneInfo.ConvertTime(reservation.EndDate,TimeZoneInfo.Utc),
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
        using var scope = _serviceProvider.CreateAsyncScope();
        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        List<Reservation> NewReservations = reservations.Select(r => new Reservation()
        {
            UserId = r.UserId,
            PrinterId = r.PrinterId,
            StartDate = TimeZoneInfo.ConvertTime(r.StartDate,TimeZoneInfo.Utc),
            EndDate = TimeZoneInfo.ConvertTime(r.EndDate,TimeZoneInfo.Utc),
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
        using var scope = _serviceProvider.CreateAsyncScope();
        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        var existingReservation = await context.Reservations
                                                .Where(r => r.ReservationId == reservationId)
                                                .FirstOrDefaultAsync();
        
        if (existingReservation != null)
        {
            existingReservation.Title = reservation.Title;
            existingReservation.Description = reservation.Description;
            existingReservation.StartDate = TimeZoneInfo.ConvertTime(reservation.StartDate,TimeZoneInfo.Utc);
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
        using var scope = _serviceProvider.CreateAsyncScope();
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
        using var scope = _serviceProvider.CreateAsyncScope();
        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        return !await context.Reservations
            .Where(r => r.PrinterId == printerId)
            .Where(r => start.ToUniversalTime() <= r.StartDate.ToUniversalTime())
            .Where(r => end.ToUniversalTime() >= r.EndDate.ToUniversalTime()).AnyAsync();
    }
    
    public async Task<bool> ValidateValidReservationWithReservationAsync(ReservationInfoDto reservation,DateTime start, DateTime end)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        return !await context.Reservations
            .Where(r => r.PrinterId == reservation.PrinterId)
            .Where(r => r.ReservationId != reservation.ReservationId)
            .Where(r => start.ToUniversalTime() <= r.StartDate.ToUniversalTime())
            .Where(r => end.ToUniversalTime() >= r.EndDate.ToUniversalTime()).AnyAsync();
    }

    public async Task PurgeOldReservationsAsync()
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        ApplicationDbContext context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

        var oldReservations = context.Reservations.Where(r => r.EndDate < DateTime.Now.ToUniversalTime()).ToList();
        
        context.Reservations.RemoveRange(oldReservations);

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
}
