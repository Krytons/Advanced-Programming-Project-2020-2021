using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            if (Password.Text == RepeatPassword.Text)
            {
                Registration log = new Registration(Email.Text, Name.Text, Surname.Text, Nickname.Text, Password.Text, RepeatPassword.Text);
                string json = JsonConvert.SerializeObject(log);
                Token token = await LoginRequest.SignIn(json);

                if (token == null)
                {
                    ErrorLabel.Text = "An error has occurred, please try again";
                    Email.Text = "";
                    Name.Text = "";
                    Surname.Text = "";
                    Nickname.Text = "";
                    Password.Text = "";
                    RepeatPassword.Text = "";
                }
                else
                {
                    try
                    {
                        await SecureStorage.SetAsync("token", token.MyToken);
                        await SecureStorage.SetAsync("email", Email.Text);
                        await Navigation.PushAsync(new SearchPage());
                    }
                    catch (Exception ex)
                    {
                        ErrorLabel.TextColor = Color.Red;
                        ErrorLabel.Text = "An error has occurred, please try again";
                        Email.Text = "";
                        Name.Text = "";
                        Surname.Text = "";
                        Nickname.Text = "";
                        Password.Text = "";
                        RepeatPassword.Text = "";
                    }
                }
            }
            else
            {
                ErrorLabel.TextColor = Color.Red;
                ErrorLabel.Text = "Passwords fields must be the same: try again";
                RepeatPassword.Text = "";
            }
          
        }

        private async void LoginRedirect(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

       
    }
}
