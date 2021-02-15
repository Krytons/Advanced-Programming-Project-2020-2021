using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{

    public class EbaySearch
    {

        [JsonProperty("search")]
        public string Search { get; set; }

        [JsonProperty("n_items")]
        public int N_items { get; set; }

        public EbaySearch(string search)
        {
            Search = search;
            N_items = 20;
        }


    }

}
