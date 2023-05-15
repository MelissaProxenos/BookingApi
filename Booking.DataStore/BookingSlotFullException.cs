namespace Booking.DataStore
{
    public class BookingSlotFullException : Exception
    {
            public BookingSlotFullException(string message)
                : base(message)
            {
            }
    }
}
