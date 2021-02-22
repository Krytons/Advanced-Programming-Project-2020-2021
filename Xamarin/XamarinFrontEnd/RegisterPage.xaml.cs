using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinFrontEnd.Classi;
using XamarinFrontEnd.HttpRequest;

namespace XamarinFrontEnd
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        private async void Registration(object sender, EventArgs e)
        {
            if (Name.Text == null || Surname.Text == null || Email.Text == null || Nickname.Text == null || Password.Text == null || RepeatPassword.Text == null)
            {
                await DisplayAlert("Try Again!", "Insert all fields", "OK");
            }
            else
            {
                if (Password.Text == RepeatPassword.Text)
                {
                    Registration log = new Registration(Email.Text, Name.Text, Surname.Text, Nickname.Text, Password.Text, RepeatPassword.Text);
                    string json = JsonConvert.SerializeObject(log);
                    HttpResponseMessage response = await LoginRequest.SignIn(json);

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
                            await DisplayAlert("Try Again!", "Something went wrong", "OK");
                        }
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                        {
                            await DisplayAlert("Attention!!!", "No connection with the server", "OK");

                        }
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            await DisplayAlert("Try Again!", "Invalid request", "OK");
                        }
                    }
                }
            }
        }

        private async void LoginRedirect(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

       
    }
}
