using PrintProxy.Hub.Services.Dto;

namespace PrintProxy.Hub.Services.reservation;

public interface IReservationService
{
    public Task<ReservationInfoDto?> GetCurrentPrinterReservationAsync(int printerId);

    public Task<List<ReservationInfoDto>> GetReservationsByPrinterAsync(int printerId);

    public Task<List<ReservationInfoDto>> GetReservationsByUserAsync(string userId);

    public Task CreateReservationAsync(EditReservationDto reservation);

    public Task EditReservationAsync(EditReservationDto reservation, int reservationId);

    public Task DeleteReservationAsync(int reservationId);
}