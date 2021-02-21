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
    public partial class RecommendationPage : ContentPage
    {

        public List<Product> Products { get; set; }

        private HttpResponseMessage response;

        public RecommendationPage()
        {
            Products = new List<Product>();
            InitializeComponent();
            FillPage();
        }

        public async void FillPage()
        {
            HttpResponseMessage response = await GetProduct.GetProductsByRecommendations();

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                List<Product> Products = JsonConvert.DeserializeObject<List<Product>>(response_content);
                MyCollectionView.ItemsSource = Products;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                {
                    await DisplayAlert("Try Again!", "No connection with the server", "OK");

                }
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Try Again!", "Invalid request", "OK");
                }
            }


        }


    }
}