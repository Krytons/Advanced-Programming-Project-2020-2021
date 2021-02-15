using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFrontEnd.Classi;
using XamarinFrontEnd.HttpRequest;
using XamarinFrontEnd.ViewModels;

namespace XamarinFrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductInfoPage : ContentPage
    {

        public Product page_product { get; set; }
        public List<Price> price_list { get; set; }

        public PlotViewModel vm;

        public ProductInfoPage(Product page_product, List<Price> price_list)
        {
            this.page_product = page_product;
            if (price_list.Any())
            {
                this.price_list = price_list;
            }
            else
            {
                this.price_list = new List<Price>() { };
            }
            InitializeComponent();

            vm = new PlotViewModel(this.price_list);
            BindingContext = vm;

            FillPage();
        }

        private void FillPage()
        {
            List<Product> products = new List<Product> { };
            products.Add(page_product);
            MyCollectionView.ItemsSource = products;

        }

        async private void InsertObservation(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("What's your desired price?", "Insert a threshold price", keyboard: Keyboard.Numeric);

            string email;

            if (result != null) {
                try
                {
                    email = await SecureStorage.GetAsync("email");
                    RequestObservation observation = new RequestObservation(page_product, result, email);
                    string json = JsonConvert.SerializeObject(observation);
                    string response = await ObservationRequest.InsertObservation(json);
                    await DisplayAlert("Success!", response, "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error!", "Something went wrong", "OK");
                }
            }
            
        }

    }
}