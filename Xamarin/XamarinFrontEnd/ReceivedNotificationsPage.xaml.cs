using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
            HttpResponseMessage response = await NotificationRequest.GetAllNotifications();

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                List<AppUserNotification> notifications_list = JsonConvert.DeserializeObject<List<AppUserNotification>>(response_content);

                foreach (AppUserNotification notification in notifications_list)
                {
                    HttpResponseMessage responseOb  = await ObservationRequest.GetObservationById(notification.Observation);

                    if (responseOb.IsSuccessStatusCode)
                    {
                        string response_contentOb = await responseOb.Content.ReadAsStringAsync();
                        RequestObservation observation = JsonConvert.DeserializeObject<RequestObservation>(response_contentOb);

                        if (observation != null)
                        {
                            OutputNotification output_value = new OutputNotification(observation, notification);
                            output_list.Add(output_value);
                        }
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                        {
                            await DisplayAlert("Attention!!!", "No connection with the server", "OK");

                        }
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            await DisplayAlert("Try Again!", "Invalid request", "OK");
                        }
                    }
                }

                Notifications = output_list;
                MyCollectionView.ItemsSource = Notifications;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                {
                    await DisplayAlert("Attention!!!", "No connection with the server", "OK");

                }
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Try Again!", "Invalid request", "OK");
                }
            }
        }

        private async void Delete(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int notification_to_delete = (int)button.CommandParameter;

            HttpResponseMessage response = await NotificationRequest.DeleteNotification(notification_to_delete);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    OutputNotification obs_to_remove = Notifications.Find(OutputNotification => OutputNotification.Notification.Id == notification_to_delete);
                    Notifications.Remove(obs_to_remove);
                    MyCollectionView.ItemsSource = null;
                    MyCollectionView.ItemsSource = Notifications;
                    await DisplayAlert("Success!", "Notification removed", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error!", "Something went wrong", "OK");
                }
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                {
                    await DisplayAlert("Attention!!!", "No connection with the server", "OK");

                }
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Try Again!", "Invalid request", "OK");
                }
            }
        }
    }

}