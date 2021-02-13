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
                Ltoken.Text = "An error has occurred, please try again";
                Password.Text = "";
                Email.Text = "";
            }
            else
            {
                try
                {
                    await SecureStorage.SetAsync("token", token.MyToken);
                    await SecureStorage.SetAsync("email", Email.Text);
                    await Navigation.PushAsync(new XamarinFrontEnd.MainPage());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Ltoken.Text = "An error has occurred, please try again";
                    Password.Text = "";
                    Email.Text = "";
                }
            }
        }


        private async void Register(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
