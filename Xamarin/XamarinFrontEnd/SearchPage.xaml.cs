using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFrontEnd.Classi;

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
            StartProvaToken();
        }

        private async void StartProvaToken()
        {
            try
            {
                var oauthToken = await SecureStorage.GetAsync("token");
                token.Text = oauthToken;
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }

        //async 
        void OnButtornPressed(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            resultsList.ItemsSource = ""; //Function that calls for ebay products
        }
    }
}
