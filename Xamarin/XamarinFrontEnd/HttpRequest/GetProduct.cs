using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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

        public static async Task<Product> GetProductById(string id)
        {
            var app = Assembly.GetAssembly(typeof(SecretClass)).GetManifestResourceStream("XamarinFrontEnd.Configuration.secrets.json");
            var stream = new StreamReader(app);
            var jsonString = stream.ReadToEnd();
            SecretClass json_des = JsonConvert.DeserializeObject<SecretClass>(jsonString.ToString());
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

            HttpResponseMessage response = await client.GetAsync(json_des + "/api/products/");

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                Product receivedProduct = JsonConvert.DeserializeObject<Product>(response_content);
                return await Task.FromResult(receivedProduct);
            }
            else return null;
        }

        public static async Task<List<Product>> GetProducts(string json)
        {

            var app = Assembly.GetAssembly(typeof(SecretClass)).GetManifestResourceStream("XamarinFrontEnd.Configuration.secrets.json");
            var stream = new StreamReader(app);
            var jsonString = stream.ReadToEnd();
            SecretClass json_des = JsonConvert.DeserializeObject<SecretClass>(jsonString.ToString());
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

            HttpResponseMessage response = await client.PostAsync(json_des + "/ebay_search", content);

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
            var app = Assembly.GetAssembly(typeof(SecretClass)).GetManifestResourceStream("XamarinFrontEnd.Configuration.secrets.json");
            var stream = new StreamReader(app);
            var jsonString = stream.ReadToEnd();
            SecretClass json_des = JsonConvert.DeserializeObject<SecretClass>(jsonString.ToString());
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

            HttpResponseMessage response = await client.GetAsync(json_des + "/price/history_by_ebay/" + product_ebay_id );
            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                List<Price> receivedProduct = JsonConvert.DeserializeObject<List<Price>>(response_content);
                return await Task.FromResult(receivedProduct);
            }
            else
            {
                List<Price> receivedProduct = new List<Price>() { };
                return await Task.FromResult(receivedProduct);
            }
        }


    }
}

