using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinFrontEnd.Classi;

namespace XamarinFrontEnd.HttpRequest
{
    public class GetProduct
    {
        public static async Task<List<Product>> GetProducts(string json)
        {

            string url = "http://302b36db2691.ngrok.io";
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

            HttpResponseMessage response = await client.PostAsync(url + "/ebay_search", content);

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                List<Product> receivedProduct = JsonConvert.DeserializeObject<List<Product>>(response_content);
                return await Task.FromResult(receivedProduct);
            }
            else return null;
        }


        public static async Task<List<Price>> GetProductPriceHistory(string product_ebay_id)
        {
            string url = "http://302b36db2691.ngrok.io";
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

            HttpResponseMessage response = await client.GetAsync(url + "/price/history_by_ebay/" + product_ebay_id );
            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                List<Price> receivedProduct = JsonConvert.DeserializeObject<List<Price>>(response_content);
                return await Task.FromResult(receivedProduct);
            }
            else return null;
        }


    }
}

