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
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

        }

        private async void GetToken(object sender, EventArgs e)
        {
            if (Email.Text == null && Password.Text == null)
            {
                await DisplayAlert("Try Again!", "Password or Email entered incorrectly", "OK");
            }
            else
            {
                Login log = new Login(Email.Text, Password.Text);
                string json = JsonConvert.SerializeObject(log);
                Token token = await LoginRequest.TryLogin(json);

                if (token == null)
                {
                    await DisplayAlert("Try Again!", "Something went wrong", "OK");
                }
                else
                {
                    try
                    {
                        await SecureStorage.SetAsync("token", token.MyToken);
                        await SecureStorage.SetAsync("email", Email.Text);
                        await Navigation.PushAsync(new MainPage());
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Try Again!", "Something went wrong", "OK");
                    }
                }
            }
        }
        private async void Register(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
