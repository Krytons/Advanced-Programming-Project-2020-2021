using System;
using UserNotifications;
using Xamarin.Forms;
using XamarinFrontEnd.Interfaces;

public class IOSNotificationReceiver : UNUserNotificationCenterDelegate
{
    public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
    {
        ProcessNotification(notification);
        completionHandler(UNNotificationPresentationOptions.Alert);
    }

    void ProcessNotification(UNNotification notification)
    {
        string title = notification.Request.Content.Title;
        string message = notification.Request.Content.Body;

        DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
    }
}