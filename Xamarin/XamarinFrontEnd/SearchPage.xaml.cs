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

        public SearchPage()
        {
            InitializeComponent();
        }

        private async void OnSearchPressed(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            EbaySearch search = new EbaySearch(searchBar.Text);
            string json = JsonConvert.SerializeObject(search);
            List<Product> products = await GetProduct.GetProducts(json);

            MyCollectionView.ItemsSource = products;
            
        }

        private async void GetMoreInfo(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Product product_to_search = (Product)button.CommandParameter;

            List<Price> prices = await GetProduct.GetProductPriceHistory(product_to_search.Item_id);

            //TODO: Navigate to product page to show product info and price history
        }

    }
}
