using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Booking.DataStore.Documents
{
    public class DocumentBase
    {
        [JsonProperty(PropertyName = "id")]
        [Description("The unique ID of the document.")]
        public string Id { get; set; }
        [Description("When the record was last updated.")]
        public DateTime LastUpdatedDate { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
