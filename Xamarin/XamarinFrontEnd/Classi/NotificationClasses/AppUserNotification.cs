using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    public class AppUserNotification
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("observation")]
        public int Observation { get; set; }

        [JsonProperty("notified_price")]
        public string Notified_price { get; set; }

        [JsonProperty("created_at")]
        public DateTime Created_at { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public AppUserNotification(int id, int observation, string notified_price, DateTime created_at, string status)
        {
            Id = id;
            Observation = observation;
            Notified_price = notified_price;
            Created_at = created_at;
            Status = status;
        }
    }
}
