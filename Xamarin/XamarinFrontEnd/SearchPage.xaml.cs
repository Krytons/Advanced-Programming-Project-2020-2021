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

        private async void OnButtornPressed(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            EbaySearch search = new EbaySearch(searchBar.Text);
            string json = JsonConvert.SerializeObject(search);

            List<Product> products = await GetProduct.GetProducts(json);
            SearchResults = new ObservableCollection<string>
            {
            };

            foreach (Product p in products)
            {
                SearchResults.Add(p.Title);
            }
            resultsList.ItemsSource = SearchResults;
        }

    }
}
