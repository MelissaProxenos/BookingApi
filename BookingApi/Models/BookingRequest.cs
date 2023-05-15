using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace Booking.Web.Models
{
    [Description("The booking details.")]
    public class BookingRequest
    {
        [JsonRequired]
        [Description("The starttime of the booking. Using format HH:mm.")]
        public string BookingTime { get; set; }

        [JsonRequired]
        [Description("The fullname of the person making the booking.")]
        public string Name { get; set; }
    }
} 
