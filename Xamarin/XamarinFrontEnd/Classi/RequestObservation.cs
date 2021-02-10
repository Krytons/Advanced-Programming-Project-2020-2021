using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFrontEnd.Classi
{
    class RequestObservation
    {
       
        public Product product { get; set; }
        public string threshold_price { get; set; }
        public string email { get; set; }

        public RequestObservation(Product product, string threshold_price, string email)
        {
            this.product = product;
            this.threshold_price = threshold_price;
            this.email = email;
        }
    }
}
