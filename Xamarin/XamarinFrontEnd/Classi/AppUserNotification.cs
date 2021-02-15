using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    public class AppUserNotification
    {
        public int id { get; set; }
        public int observation { get; set; }
        public string notified_price { get; set; }
        public DateTime created_at { get; set; }
        public string status { get; set; }

        public AppUserNotification(int id, int observation, string notified_price, DateTime created_at, string status)
        {
            this.id = id;
            this.observation = observation;
            this.notified_price = notified_price;
            this.created_at = created_at;
            this.status = status;
        }
    }
}
