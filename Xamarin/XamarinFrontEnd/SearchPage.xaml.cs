using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinFrontEnd
{
    public partial class SearchPage : ContentPage
    {
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
    }
}
