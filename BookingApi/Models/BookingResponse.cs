using System.ComponentModel;

namespace Booking.Web.Models
{
    public class BookingResponse
    {
        [Description("The unique referenec number for the booking made. The reference will be returned in format XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX")]
        public Guid BookingId { get; set; }
    }
}
