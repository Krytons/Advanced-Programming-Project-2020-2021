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
            /*
            int rowIndex = 0;
            foreach(RequestObservation element in CompleteObservations)
            {
                var image_element = new Image
                {
                    Source = element.product.Gallery_url,
                    Aspect = Aspect.AspectFill,
                    HeightRequest = 130,
                    WidthRequest = 130
                };
                GridLayout.Children.Add(image_element, 0, rowIndex);
                Grid.SetRowSpan(image_element, 2);
                var product_title_label = new Label
                {
                    Text = element.product.Title,
                    FontAttributes = FontAttributes.Italic,
                    VerticalOptions = LayoutOptions.End
                };
                GridLayout.Children.Add(product_title_label, 1, rowIndex);
                var price_label = new Label
                {
                    Text = "Threshold: €" + element.threshold_price,
                    FontAttributes = FontAttributes.Bold
                };
                GridLayout.Children.Add(price_label, 1, (rowIndex+1));

                rowIndex = rowIndex + 2;
            }
            */
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
                    RequestObservation obs_to_remove = CompleteObservations.Find(RequestObservation => RequestObservation.product.Id == observation_product);
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