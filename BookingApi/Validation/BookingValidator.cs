using Booking.Web.Models;

namespace Booking.Web.Validation
{
    public static class BookingValidator
    {
        public const string TimeFormat = "HH:mm";
        public static void ValidateBookingRequest(BookingRequest bookingRequest)
        {
            if (bookingRequest == null)
            {
                throw new BadHttpRequestException("Validation Error: Booking request is empty");
            }

            if (string.IsNullOrWhiteSpace(bookingRequest.Name))
            {
                throw new BadHttpRequestException("Validation Error: Name is empty");
            }

            if(TimeOnly.TryParseExact(bookingRequest.BookingTime, TimeFormat, out var bookingTime))
            {
                var nineAM = new TimeOnly(9, 0);
                var fourPM = new TimeOnly(16, 0);
                if (!bookingTime.IsBetween(nineAM, fourPM))
                {
                    throw new BadHttpRequestException("Validation Error: Outside of business hours");
                }
            }
            else
            {
                throw new BadHttpRequestException("Validation Error: Invalid booking time");
            }
            
        }
    }
}
