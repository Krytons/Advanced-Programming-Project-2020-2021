using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFrontEnd.Classi;
using XamarinFrontEnd.HttpRequest;
using XamarinFrontEnd.Interfaces;

namespace XamarinFrontEnd
{
    public partial class App : Application
    {

        private static Stopwatch stopWatch = new Stopwatch();
        private const int defaultTimespan = 1;

        INotificationManager notificationManager;
        int notificationNumber = 0;

        public App()
        {
            InitializeComponent();

            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };

            MainPage = new NavigationPage(new LoginPage());
   
        }

        protected override void OnStart()
        {
            SendNotification();
            // On start runs when your application launches from a closed state
            if (!stopWatch.IsRunning)
            {
                stopWatch.Start();
            }

            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                //Logic for logging out if the device is inactive for a period of time
                if (stopWatch.IsRunning && stopWatch.Elapsed.Minutes >= defaultTimespan)
                {
                    //Try to call the server to obtain all not-pulled notifications 
                    SendNotification();

                    stopWatch.Restart();
                }

                // Always return true as to keep our device timer running.
                return true;
            });
        }

        protected override void OnSleep()
        {
            //Ensure our stopwatch is reset so the elapsed time is 0.
            stopWatch.Reset();
        }

        protected override void OnResume()
        {
            //App enters the foreground so start our stopwatch again.
            stopWatch.Start();
        }

        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var msg = new Label()
                {
                    Text = $"Notification Received:\nTitle: {title}\nMessage: {message}"
                };

            });
        }

        async void SendNotification()
        {
            List<AppUserNotification> user_notifications = await NotificationRequest.GetNotPulledNotifications();
            if (user_notifications.Any())
            {
                foreach (AppUserNotification user_notification in user_notifications)
                {
                    RequestObservation observation = await ObservationRequest.GetObservationById(user_notification.Observation);
                    string title = "🚨 Good news! 🚨";
                    string message = "💰 Your observed product: " + observation.Product.Title + " is now available for: €" + observation.Product.Price + " 💰";
                    notificationManager.SendNotification(title, message);
                }
            }
        }
    }
}
