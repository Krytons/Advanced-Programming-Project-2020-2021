using System;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinFrontEnd.Classi;
using XamarinFrontEnd.HttpRequest;

namespace XamarinFrontEnd
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void GetToken(object sender, EventArgs e)
        {

                Login log = new Login(Email.Text, Password.Text);
                string json = JsonConvert.SerializeObject(log);
                Token token = await LoginRequest.TryLogin(json);

                if (token == null)
                {
                    Ltoken.Text = "Error";
                }
                else
                {
                    try
                    {
                        await SecureStorage.SetAsync("token", token.MyToken);
                    }
                    catch (Exception ex)
                    {
                        // Possible that device doesn't support secure storage on device.
                    }
                }

                await Navigation.PushAsync(new SearchPage());
            }
        }
    }
}
