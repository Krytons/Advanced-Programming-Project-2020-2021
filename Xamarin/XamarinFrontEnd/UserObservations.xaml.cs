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
    public partial class UserObservations : ContentPage
    {

        public List<RequestObservation> CompleteObservations { get; set; }

        public UserObservations()
        {
            InitializeComponent();
            FillPage();
        }

        private async void FillPage()
        {
            HttpResponseMessage response = await ObservationRequest.GetAllUserObservation();

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                List<RequestObservation> CompleteObservations = JsonConvert.DeserializeObject<List<RequestObservation>>(response_content);
                MyCollectionView.ItemsSource = CompleteObservations;
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

        private void OnFavoriteSwipeItemInvoked(object sender, EventArgs e)
        {

        }

        private async void Delete(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string observation_product = (string)button.CommandParameter;
            HttpResponseMessage response = await ObservationRequest.DeleteObservation(observation_product);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    RequestObservation obs_to_remove = CompleteObservations.Find(RequestObservation => RequestObservation.Product.Id == observation_product);
                    CompleteObservations.Remove(obs_to_remove);
                    Console.WriteLine(CompleteObservations);
                    MyCollectionView.ItemsSource = null;
                    MyCollectionView.ItemsSource = CompleteObservations;
                    await DisplayAlert("Success!", "Observation removed", "OK");
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