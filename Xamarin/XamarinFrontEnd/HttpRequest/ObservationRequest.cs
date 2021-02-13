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

            string url = "http://c135d0e6a5e8.ngrok.io";
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

        public static async Task<List<RequestObservation>> GetAllUserObservation()
        {
            string url = "http://c135d0e6a5e8.ngrok.io";
            string token = null;
            HttpClient client = new HttpClient();

            try
            {
                token = await SecureStorage.GetAsync("token");
            }
            catch (Exception ex)
            {
                return null;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            HttpResponseMessage response = await client.GetAsync(url + "/get_complete_user_observation_data");

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                List<RequestObservation> receivedList  = JsonConvert.DeserializeObject<List<RequestObservation>>(response_content);
                return await Task.FromResult(receivedList);
            }
            else return null;
        }

        public static async Task<string> DeleteObservation(string product_id)
        {
            string url = "http://c135d0e6a5e8.ngrok.io";
            string token = null;
            HttpClient client = new HttpClient();

            try
            {
                token = await SecureStorage.GetAsync("token");
            }
            catch (Exception ex)
            {
                return null;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            HttpResponseMessage response = await client.DeleteAsync(url + "/delete_observation_by_product_id/"+product_id);

            if (response.IsSuccessStatusCode)
            {
                return await Task.FromResult("Observation deleted!");
            }
            else return null;
        }
    }
}
