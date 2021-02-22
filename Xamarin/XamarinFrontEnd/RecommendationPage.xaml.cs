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
                Products = JsonConvert.DeserializeObject<List<Product>>(response_content);
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

        private async void GetMoreInfo(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string product_to_search = (string)button.CommandParameter;
            Product button_product = Products.Find(Products => Products.Item_id == product_to_search);
            HttpResponseMessage response = await GetProduct.GetProductPriceHistory(button_product.Item_id);
            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                List<Price> prices = JsonConvert.DeserializeObject<List<Price>>(response_content);
                await Navigation.PushAsync(new ProductInfoPage(button_product, prices));
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                {
                    await DisplayAlert("Try Again!", "No connection with the server", "OK");

                }
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    List<Price> prices = new List<Price>();
                    await Navigation.PushAsync(new ProductInfoPage(button_product, prices));
                }
            }
        }


    }
}