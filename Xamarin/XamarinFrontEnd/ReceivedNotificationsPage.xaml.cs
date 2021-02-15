using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFrontEnd.Classi;
using XamarinFrontEnd.HttpRequest;

namespace XamarinFrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceivedNotificationsPage : ContentPage
    {
        
        public List<OutputNotification> Notifications { get; set; }

        public ReceivedNotificationsPage()
        {
   
            InitializeComponent();
            FillPage();
        }

        private async void FillPage()
        {

            List<OutputNotification> output_list  = new List<OutputNotification>();
            List<AppUserNotification> notifications_list = await NotificationRequest.GetAllNotifications();

            foreach (AppUserNotification notification in notifications_list)
            {
                RequestObservation observation = await ObservationRequest.GetObservationById(notification.Observation);
                if (observation != null)
                {
                    OutputNotification output_value = new OutputNotification(observation, notification);
                    output_list.Add(output_value);
                }
            }

            Notifications = output_list;
            MyCollectionView.ItemsSource = Notifications;
        }

        private void Delete(object sender, EventArgs e)
        {

        }
    }

}