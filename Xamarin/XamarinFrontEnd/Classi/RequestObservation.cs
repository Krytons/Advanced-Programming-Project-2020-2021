using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    public class RequestObservation
    {

        [JsonProperty("product")]
        public Product Product { get; set; }

        [JsonProperty("threshold_price")]
        public string Threshold_price { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        public RequestObservation(Product product, string threshold_price, string email)
        {
            Product = product;
            Threshold_price = threshold_price;
            Email = email;
        }
    }
}
