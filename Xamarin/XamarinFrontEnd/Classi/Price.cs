using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    public class Price
    {

        public int id { get; set; }
        public int product { get; set; }
        public string old_price { get; set; }
        public DateTime price_time { get; set; }

        public Price(int id, int product, string old_price, DateTime price_time)
        {
            this.id = id;
            this.product = product;
            this.old_price = old_price;
            this.price_time = price_time;
        }
    }
}
