using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{

    public class EbaySearch
    {
        
        public string search { get; set; }
        public int n_items { get; set; }

        public EbaySearch(string search, int n_items)
        {
            this.search = search;
            this.n_items = n_items;
        }
    }

}
