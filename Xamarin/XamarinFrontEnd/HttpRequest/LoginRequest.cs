using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinFrontEnd.Classi;

namespace XamarinFrontEnd.HttpRequest
{
    public class LoginRequest
    {

        public static async Task<Token> TryLogin(string json)
        {

            string url = "http://a1e47ca7d9e6.ngrok.io";

            HttpClient client = new HttpClient();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                Token receivedProduct = JsonConvert.DeserializeObject<Token>(response_content);
                return await Task.FromResult(receivedProduct);
            }
            else return null;
        }
    }
}
