using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    public class OutputNotification
    {
        
        public RequestObservation Observation { get; set; }
        public AppUserNotification Notification { get; set; }

        public OutputNotification(RequestObservation observation, AppUserNotification notification)
        {
            Observation = observation;
            Notification = notification;
        }

    }
}
