using Booking.DataStore.Helpers;
using Booking.DataStore.Interfaces;

namespace Booking.DataStore.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;


    public BookingRepository(ILogger<BookingRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<string> AddAsync(Documents.Booking booking)
        {
            var bookingsInSlot =
                BookingsHelper.BookingsList.Count(b => b.BookingStartTime.IsBetween(booking.BookingStartTime, booking.BookingEndTime) || b.BookingEndTime.IsBetween(booking.BookingStartTime, booking.BookingEndTime));
            if (bookingsInSlot >= 4)
            {
                _logger.LogError($"Booking slots are full for {booking.BookingStartTime} - {booking.BookingEndTime}");
                throw new BookingSlotFullException($"Booking slots are full for {booking.BookingStartTime} - {booking.BookingEndTime}");
            }
            booking.Id = Guid.NewGuid().ToString();
            booking.LastUpdatedDate = DateTime.UtcNow;
            // TODO Do an await here when this becomes an IO DB Call 
            BookingsHelper.BookingsList.Add(booking);
            return booking.Id;
        }
    }
}
