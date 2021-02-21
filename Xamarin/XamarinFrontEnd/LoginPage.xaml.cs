using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinFrontEnd.Classi;
using XamarinFrontEnd.HttpRequest;

namespace XamarinFrontEnd
{
    public partial class LoginPage : ContentPage
    {

        public delegate void Del(string control);
        public static string control;

        public static void DelegateMethod(string message)
        {
            control = message;
        }

        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

        }

        private async void GetToken(object sender, EventArgs e)
        {
            Del control = DelegateMethod;

            if (Email.Text == null || Password.Text == null)
            {
                await DisplayAlert("Try Again!", "Password or Email entered incorrectly", "OK");
            }
            else
            {
                Login log = new Login(Email.Text, Password.Text);
                string json = JsonConvert.SerializeObject(log);
                HttpResponseMessage response = await LoginRequest.TryLogin(json);

                if (response.IsSuccessStatusCode)
                {
                    string response_content = await response.Content.ReadAsStringAsync();
                    Token token = JsonConvert.DeserializeObject<Token>(response_content);

                    try
                    {
                        await SecureStorage.SetAsync("token", token.MyToken);
                        await SecureStorage.SetAsync("email", Email.Text);
                        await Navigation.PushAsync(new MainPage());
                    }
                    catch (Exception ex)
                    {
                            await DisplayAlert("Attention!!!", "Something went wrong", "OK");
                    }
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
        }
        private async void Register(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
