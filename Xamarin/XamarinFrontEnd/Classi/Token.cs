using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace XamarinFrontEnd.Classi
{
    public class Token
    {
        [JsonProperty("token")]
        private string myToken;


        public Token(string token)
        {
            this.myToken = token;
        }

        public string MyToken
        {
            get { return myToken; }
            set { myToken = value; }
        }
    }
}

