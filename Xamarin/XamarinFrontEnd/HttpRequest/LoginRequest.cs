﻿using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinFrontEnd.Classi;
using static XamarinFrontEnd.LoginPage;

namespace XamarinFrontEnd.HttpRequest
{
    public class LoginRequest
    {

        public static async Task<HttpResponseMessage> TryLogin(string json)
        {

            var app = Assembly.GetAssembly(typeof(SecretClass)).GetManifestResourceStream("XamarinFrontEnd.Configuration.secrets.json");
            var stream = new StreamReader(app);
            var jsonString = stream.ReadToEnd();
            SecretClass json_des = JsonConvert.DeserializeObject<SecretClass>(jsonString.ToString());
         
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await client.PostAsync(json_des.Ngrok + "/login", content);
            return await Task.FromResult(response);
        }

        public static async Task<HttpResponseMessage> SignIn(string json)
        {
            var app = Assembly.GetAssembly(typeof(SecretClass)).GetManifestResourceStream("XamarinFrontEnd.Configuration.secrets.json");
            var stream = new StreamReader(app);
            var jsonString = stream.ReadToEnd();
            SecretClass json_des = JsonConvert.DeserializeObject<SecretClass>(jsonString.ToString());
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(json_des.Ngrok + "/register", content);

            return await Task.FromResult(response);

        }
    }
}
