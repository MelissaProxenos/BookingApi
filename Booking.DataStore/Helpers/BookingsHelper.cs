namespace Booking.DataStore.Helpers
{
    public static class BookingsHelper
    {
        static BookingsHelper()
        {
            BookingsList = new List<Documents.Booking>();
        }

        public static IList<Documents.Booking> BookingsList { get; set; }
    }
}
