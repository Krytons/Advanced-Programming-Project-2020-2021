using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<string> SearchResults { get; set; }
        public List<ObservableCollection<string>> ListItems { get; set; }

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
            Products = await GetProduct.GetProducts(json);

            MyCollectionView.ItemsSource = Products;
            
        }

        private async void GetMoreInfo(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string product_to_search = (string)button.CommandParameter;
            Product button_product = Products.Find(Products => Products.Item_id == product_to_search);

            List<Price> prices = await GetProduct.GetProductPriceHistory(button_product.Item_id);

            //TODO: Navigate to product page to show product info and price history
        }

    }
}
