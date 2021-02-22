using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public Product Page_product { get; set; }
        public List<Price> Price_list { get; set; }

        public PlotViewModel vm;

        public ProductInfoPage(Product page_product, List<Price> price_list)
        {
            this.Page_product = page_product;
            if (price_list.Any())
            {
                Price_list = price_list;
            }
            else
            {
                Price_list = new List<Price>() { };
            }
            InitializeComponent();

            vm = new PlotViewModel(this.Price_list);
            BindingContext = vm;

            FillPage();
        }

        private void FillPage()
        {
            List<Product> products = new List<Product> { };
            products.Add(Page_product);
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
                    RequestObservation observation = new RequestObservation(Page_product, result, email);
                    string json = JsonConvert.SerializeObject(observation);
                    HttpResponseMessage response = await ObservationRequest.InsertObservation(json);

                    if (response.IsSuccessStatusCode)
                    {
                        string response_content = await response.Content.ReadAsStringAsync();
                        await DisplayAlert("Success!", "Observation successful", "OK");
                    }
                    //VEDERE SE VA BENE O TOGLIERE TRY CATCH
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                        {
                            await DisplayAlert("Try Again!", "No connection with the server", "OK");

                        }
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            await DisplayAlert("Try Again!", "Invalid request", "OK");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error!", "Something went wrong", "OK");
                }
            }
            
        }

    }
}