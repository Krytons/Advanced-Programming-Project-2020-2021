using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    public class Price
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("product")]
        public int Product { get; set; }

        [JsonProperty("old_price")]
        public string Old_price { get; set; }

        [JsonProperty("price_time")]
        public DateTime Price_time { get; set; }

        public Price(int id, int product, string old_price, DateTime price_time)
        {
            Id = id;
            Product = product;
            Old_price = old_price;
            Price_time = price_time;
        }
    }
}
