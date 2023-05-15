namespace Booking.DataStore.Interfaces
{
    public interface IBookingRepository
    {
        Task<string> AddAsync(Documents.Booking booking);
    }
}
