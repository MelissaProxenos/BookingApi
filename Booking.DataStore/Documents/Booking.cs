using System.ComponentModel;

namespace Booking.DataStore.Documents
{
    public class Booking :DocumentBase 
    {
        [Description("The start time of the booking. Using format HH:mm.")]
        public TimeOnly BookingStartTime { get; set; }
        [Description("The end time of the booking. Using format HH:mm.")]
        public TimeOnly BookingEndTime { get; set; }
        [Description("The fullname of the person making the booking.")]
        public string Name { get; set; }
    }
}
