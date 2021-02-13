using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XamarinFrontEnd.Classi;

namespace XamarinFrontEnd.HttpRequest
{
    public class ObservationRequest
    {
        public static async Task<String> InsertObservation(string json)
        {

            string url = "http://8e0349b45fc8.ngrok.io";
            string token = null;
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                token = await SecureStorage.GetAsync("token");
            }
            catch (Exception ex)
            {
                return null;
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            HttpResponseMessage response = await client.PostAsync(url + "/ebay_select", content);

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                return "Observation successful";
            }
            else return null;
        }
    }
}
