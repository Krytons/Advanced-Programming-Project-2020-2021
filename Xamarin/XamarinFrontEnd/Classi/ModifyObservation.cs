using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    public class ModifyObservation
    {
       
        [JsonProperty("creator")]
        public string Creator { get; set; }

        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("threshold_price")]
        public string Threshold_price { get; set; }

        public ModifyObservation(string creator, string product, string threshold_price)
        {
            Creator = creator;
            Product = product;
            Threshold_price = threshold_price;
        }

    }
}
