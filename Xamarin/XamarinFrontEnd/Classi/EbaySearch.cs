using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{

    public class EbaySearch
    {
        
        public string search { get; set; }
        public int n_items { get; set; }

        public EbaySearch(string search)
        {
            this.search = search;
            this.n_items = 20;
        }
    }

}
