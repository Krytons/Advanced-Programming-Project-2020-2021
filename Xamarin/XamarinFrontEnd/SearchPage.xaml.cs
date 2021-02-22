using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFrontEnd.Classi;
using XamarinFrontEnd.HttpRequest;

namespace XamarinFrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {

        public List<Product> Products { get; set; }

        public SearchPage()
        {
            InitializeComponent();
        }

        private async void OnSearchPressed(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            EbaySearch search = new EbaySearch(searchBar.Text);
            string json = JsonConvert.SerializeObject(search);
            HttpResponseMessage response = await GetProduct.GetProducts(json);

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
