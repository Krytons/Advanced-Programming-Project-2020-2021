using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace XamarinFrontEnd.Classi
{
    public class Login
    {
        [JsonProperty("username")]
        private string username;

        [JsonProperty("password")]
        private string password;

        public Login(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string Username
        {
            get { return username; } 
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

    }
}

