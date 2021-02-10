using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XamarinFrontEnd.Classi
{
    class Product
    {

        [JsonProperty("id")]
        private int id;

        [JsonProperty("item_id")]
        private string item_id;

        [JsonProperty("title")]
        private string title;

        [JsonProperty("subtitle")]
        private string subtitle;

        [JsonProperty("category_id")]
        private string category_id;

        [JsonProperty("category_id")]
        private string category_name;

        [JsonProperty("gallery_url")]
        private string gallery_url;

        [JsonProperty("view_url")]
        private string view_url;

        [JsonProperty("shipping_cost")]
        private double shipping_cost;

        [JsonProperty("price")]
        private double price;

        [JsonProperty("condition_id")]
        private string condition_id;

        [JsonProperty("condition_name")]
        private string condition_name;

        [JsonProperty("created_at")]
        private DateTime created_at;

        [JsonProperty("updated_at")]
        private DateTime updated_at;

        public Product(int id, string item_id, string title, string subtitle, string category_id, string category_name, string gallery_url, string view_url, double shipping_cost, double price, string condition_id, string condition_name, DateTime created_at, DateTime updated_at)
        {
            this.id = id;
            this.item_id = item_id;
            this.title = title;
            this.subtitle = subtitle;
            this.category_id = category_id;
            this.category_name = category_name;
            this.gallery_url = gallery_url;
            this.view_url = view_url;
            this.shipping_cost = shipping_cost;
            this.price = price;
            this.condition_id = condition_id;
            this.condition_name = condition_name;
            this.created_at = created_at;
            this.updated_at = updated_at;
        }

        // Getters and setters
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Item_id
        {
            get { return item_id; }
            set { item_id = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Subtitle
        {
            get { return subtitle; }
            set { subtitle = value; }
        }

        public string Category_id
        {
            get { return category_id; }
            set { category_id = value; }
        }

        public string Category_name
        {
            get { return category_name; }
            set { category_name = value; }
        }

        public string Gallery_url
        {
            get { return gallery_url; }
            set { gallery_url = value; }
        }

        public string View_url
        {
            get { return view_url; }
            set { view_url = value; }
        }

        public double Shipping_cost
        {
            get { return shipping_cost; }
            set { shipping_cost = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Condition_id
        {
            get { return condition_id; }
            set { condition_id = value; }
        }

        public string Condition_name
        {
            get { return condition_name; }
            set { condition_name = value; }
        }

        public DateTime Created_at
        {
            get { return created_at; }
            set { created_at = value; }
        }

        public DateTime Updated_at
        {
            get { return updated_at; }
            set { updated_at = value; }
        }

        public DateTime Updated_At { get; }
    }
}
