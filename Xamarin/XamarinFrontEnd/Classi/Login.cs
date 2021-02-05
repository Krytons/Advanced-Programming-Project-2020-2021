using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace XamarinFrontEnd.Classi
{
    public class Login
    {
        [JsonProperty("email")]
        private string email;

        [JsonProperty("password")]
        private string password;

        public Login(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

    }
}

