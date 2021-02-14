using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
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

            var app = Assembly.GetAssembly(typeof(SecretClass)).GetManifestResourceStream("XamarinFrontEnd.Configuration.secrets.json");
            var stream = new StreamReader(app);
            var jsonString = stream.ReadToEnd();
            SecretClass json_des = JsonConvert.DeserializeObject<SecretClass>(jsonString.ToString());
         
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await client.PostAsync(json_des.Ngrok + "/login", content);

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                Token receivedToken = JsonConvert.DeserializeObject<Token>(response_content);
                return await Task.FromResult(receivedToken);
            }
            else return null;
        }

        public static async Task<Token> SignIn(string json)
        {
            var app = Assembly.GetAssembly(typeof(SecretClass)).GetManifestResourceStream("XamarinFrontEnd.Configuration.secrets.json");
            var stream = new StreamReader(app);
            var jsonString = stream.ReadToEnd();
            SecretClass json_des = JsonConvert.DeserializeObject<SecretClass>(jsonString.ToString());
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(json_des.Ngrok + "/register", content);

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                RegistrationResponse receivedRegistration = JsonConvert.DeserializeObject<RegistrationResponse>(response_content);
                Token receivedToken = new Token(receivedRegistration.token);
                return await Task.FromResult(receivedToken);
            }
            else return null;

        }
    }
}
