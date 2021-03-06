﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XamarinFrontEnd.Classi;

namespace XamarinFrontEnd.HttpRequest
{
    public class ObservationRequest
    {
        public static async Task<HttpResponseMessage> InsertObservation(string json)
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
            HttpResponseMessage response = await client.PostAsync(json_des.Ngrok + "/ebay_select", content);

            return await Task.FromResult(response);
        }


        public static async Task<HttpResponseMessage> GetObservationById(int observation_id)
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
            HttpResponseMessage response = await client.GetAsync(json_des.Ngrok + "/get_user_observation_data_by_id/" + observation_id);

            return await Task.FromResult(response);
        }


        public static async Task<HttpResponseMessage> GetAllUserObservation()
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
            HttpResponseMessage response = await client.GetAsync(json_des.Ngrok + "/get_complete_user_observation_data");

            return await Task.FromResult(response);
        }

        public static async Task<HttpResponseMessage> DeleteObservation(string product_id)
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
            HttpResponseMessage response = await client.DeleteAsync(json_des.Ngrok + "/delete_observation_by_product_id/"+product_id);

            return await Task.FromResult(response);
        }

        public static async Task<HttpResponseMessage> UpdateObservation(string json_body, string obs_id)
        {
            var app = Assembly.GetAssembly(typeof(SecretClass)).GetManifestResourceStream("XamarinFrontEnd.Configuration.secrets.json");
            var stream = new StreamReader(app);
            var jsonString = stream.ReadToEnd();
            SecretClass json_des = JsonConvert.DeserializeObject<SecretClass>(jsonString.ToString());
            string token = null;
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(json_body, Encoding.UTF8, "application/json");

            try
            {
                token = await SecureStorage.GetAsync("token");
            }
            catch (Exception ex)
            {
                return null;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            HttpResponseMessage response = await client.PutAsync(json_des.Ngrok + "/update_observation_by_product_id/" + obs_id, content);

            return await Task.FromResult(response);

        }
    }
}
