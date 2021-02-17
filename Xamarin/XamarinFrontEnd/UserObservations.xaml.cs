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
            CompleteObservations = await ObservationRequest.GetAllUserObservation();
            MyCollectionView.ItemsSource = CompleteObservations;
        }

        private void OnFavoriteSwipeItemInvoked(object sender, EventArgs e)
        {

        }

        private async void Delete(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string observation_product = (string)button.CommandParameter;
            string response = await ObservationRequest.DeleteObservation(observation_product);

            if (response != null)
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
        }


    }

}